namespace Syncra.Components;

public interface INetworkedComponent
{
    void WriteAll(Stream stream);
    void ReadAll(Stream stream);
}
