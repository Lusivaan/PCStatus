using System.Runtime.InteropServices; //P/Invoke, mecanismo que conecta o C# a funções nativas do Windows

namespace PCStatus.Services;
internal class MemoryService
{
   [DllImport("kernel32.dll")] //DllImport - funcao que procura a dll //kernel32.dll - dll nativa do windows
    static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

    [StructLayout(LayoutKind.Sequential)] //mantém os campos na ordem declarada, como a API do Windows espera.
    struct MEMORYSTATUSEX //struct - define uma estrutura que agrupa dados
    {
        public uint dwLength;
        public uint dwMemoryLoad; //porcentagem de memória utilizada
        public ulong ullTotalPhys; //RAM total
        public ulong ullAvailPhys; //RAM disponível
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    } 
    
    public uint ObterUsoRam()
    {
        MEMORYSTATUSEX memoria = new MEMORYSTATUSEX();
        memoria.dwLength = (uint)Marshal.SizeOf<MEMORYSTATUSEX>();

        bool sucesso = GlobalMemoryStatusEx(ref memoria);

        if(!sucesso)
            throw new InvalidOperationException("Erro ao consultar a RAM.");

        return memoria.dwMemoryLoad;
    }
}


