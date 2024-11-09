namespace Syncra.Components;

public interface INetworkedComponent
{
    void WriteAll(Stream stream);
    void ReadAll(Stream stream);
    
    bool WriteUpdates(Stream stream);
    void ReadUpdates(Stream stream);
}
