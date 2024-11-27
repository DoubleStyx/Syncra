using Godot;

public partial class CharacterController : CharacterBody3D
{
    [Export] public float Speed = 5.0f;
    [Export] public float Gravity = -9.8f;
    [Export] public float JumpForce = 10.0f;
    [Export] public float Sensitivity = 0.1f;

    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _rotation = Vector3.Zero;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            _rotation.X -= mouseMotion.Relative.X * Sensitivity;
            _rotation.X -= mouseMotion.Relative.Y * Sensitivity;
            _rotation.X = Mathf.Clamp(_rotation.X, -80, 80);

            RotationDegrees = new Vector3(0, _rotation.Y, 0);
            GetNode<Camera3D>("Camera").RotationDegrees = new Vector3(_rotation.X, 0, 0);
        }
    }

    public void _PhysicsProcess(float delta)
    {
        _velocity.Y += Gravity * delta;

        // Movement input
        var direction = Vector3.Zero;
        if (Input.IsActionPressed("ui_up"))
            direction -= Transform.Basis.Z;
        if (Input.IsActionPressed("ui_down"))
            direction += Transform.Basis.Z;
        if (Input.IsActionPressed("ui_left"))
            direction -= Transform.Basis.X;
        if (Input.IsActionPressed("ui_right"))
            direction += Transform.Basis.X;

        if (direction.Length() > 0)
            direction = direction.Normalized();

        _velocity.X = direction.X * Speed;
        _velocity.Z = direction.Z * Speed;

        MoveAndSlide();

        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            _velocity.Y = JumpForce;
        }
    }
}