using Silk.NET.GLFW;
using Silk.NET.Vulkan;

namespace Syncra.Drivers;

/// <summary>
///     The Vulkan renderer driver, using MoltenVK for macOS support. Maybe eventually support Metal or DX12 natively.
/// </summary>
public class Renderer : IDisposable
{
    private readonly Instance _instance;
    private readonly Glfw _glfw;
    private readonly Vk _vk;
    private readonly byte[] _name = "Syncra"u8.ToArray();

    private readonly byte[][] _validationLayers =
    [
        "VK_LAYER_KHRONOS_validation"u8.ToArray()
    ];

    /// <summary>
    ///     Initializes the Vulkan renderer.
    /// </summary>
    public Renderer()
    {
        _glfw = Glfw.GetApi();
        _vk = Vk.GetApi();
        
        var applicationInfo = new ApplicationInfo
        {
            SType = StructureType.ApplicationInfo
        };
        
        unsafe
        {
            fixed (byte* pName = _name)
            {
                applicationInfo.PApplicationName = pName;
                applicationInfo.PEngineName = pName;
            }
        }

        applicationInfo.ApplicationVersion = 1;
        applicationInfo.ApiVersion = 1;
        applicationInfo.EngineVersion = 1;
        var instanceCreateInfo = new InstanceCreateInfo();

        unsafe
        {
            instanceCreateInfo.PApplicationInfo = &applicationInfo;
        }

        uint glfwExtensionCount = 0;
        
        unsafe
        {
            var extensionsPtr = _glfw.GetRequiredInstanceExtensions(out glfwExtensionCount);
            instanceCreateInfo.PpEnabledExtensionNames = extensionsPtr;
        }

        instanceCreateInfo.EnabledExtensionCount = glfwExtensionCount;
        instanceCreateInfo.EnabledLayerCount = (uint)_validationLayers.Length;

        unsafe
        {
            var validationLayerPointers = new byte*[_validationLayers.Length];
            for (var i = 0; i < _validationLayers.Length; i++)
                fixed (byte* pLayerName = _validationLayers[i])
                {
                    validationLayerPointers[i] = pLayerName;
                }

            fixed (byte** pValidationLayers = validationLayerPointers)
            {
                instanceCreateInfo.PpEnabledLayerNames = pValidationLayers;
            }
        }

        unsafe
        {
            if (_vk.CreateInstance(&instanceCreateInfo, null, out _instance) != Result.Success)
                throw new Exception("Failed to create Vulkan instance.");
        }
        
        uint physicalDeviceCount = 0;
        
        unsafe
        {
            _vk.EnumeratePhysicalDevices(_instance, &physicalDeviceCount, null);
        }

        if (physicalDeviceCount == 0) throw new Exception("Failed to find GPUs with Vulkan support.");
        var physicalDevices = new PhysicalDevice[physicalDeviceCount];
        
        unsafe
        {
            fixed (PhysicalDevice* pPhysicalDevices = physicalDevices)
            {
                _vk.EnumeratePhysicalDevices(_instance, &physicalDeviceCount, pPhysicalDevices);
            }
        }

        var physicalDevice = physicalDevices[0];
        
        
    }

    /// <summary>
    ///     Runs the renderer loop.
    /// </summary>
    public static void Run()
    {
    }

    /// <summary>
    ///     Disposes the Vulkan instance.
    /// </summary>
    public void Dispose()
    {
        if (_instance.Handle != IntPtr.Zero)
            unsafe
            {
                _vk.DestroyInstance(_instance, null);
            }

        _vk.Dispose();
    }
}