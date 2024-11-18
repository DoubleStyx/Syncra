using System.Runtime.InteropServices;
using Silk.NET.GLFW;
using Silk.NET.Vulkan;

namespace Syncra.Drivers;

/// <summary>
///     The Vulkan renderer driver, using MoltenVK for macOS support. Maybe eventually support Metal or DX12 natively.
/// </summary>
public class Renderer : IDisposable
{
    private Instance _instance;
    private Glfw _glfw;
    private Vk _vk;
    private readonly byte[] _name = "Syncra"u8.ToArray();
    private readonly byte[][] _validationLayers =
    {
        "VK_LAYER_KHRONOS_validation"u8.ToArray(),
    };

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
            fixed (byte** pValidationLayers = _validationLayers)
            {
                instanceCreateInfo.PpEnabledLayerNames = _validationLayers;
            }
        }

        unsafe
        {
            if (_vk.CreateInstance(&instanceCreateInfo, null, out _instance) != Result.Success)
                throw new Exception("Failed to create Vulkan instance.");
        }
    }

    /// <summary>
    ///     Runs the renderer loop.
    /// </summary>
    public static void Run()
    {
    }

    /// <summary>
    /// Disposes the Vulkan instance.
    /// </summary>
    public void Dispose()
    {
        _vk.Dispose();
    }
}