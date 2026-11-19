using System.Diagnostics; //permite consultar contadores de desempenho do Windows, como CPU, memória e disco.
using PCStatus.Services;

internal static class Program
{
    static void Main()
    {
        bool continuar = true;
        Console.CancelKeyPress += (sender, e) => //evento disparado quando o usuário pressiona Ctrl+C
        {
            e.Cancel= true;
            continuar = false;

        };

        PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        PerformanceCounter discoPorcent = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
        
        DriveInfo disco = new DriveInfo("C");
        long total = disco.TotalSize;
        long livre = disco.TotalFreeSpace;

        MemoryService ram = new MemoryService();

        Process explorer = Process.GetProcessesByName("explorer").First();
        DateTime inicioSessao = explorer.StartTime;  
        
        
        
        while (continuar)
        {   
            Console.Clear();
            Console.SetCursorPosition(0, 0); //move o cursor para a primeira coluna e primeira linha

            Process[] processos = Process.GetProcesses(); // retorna os processos em execução no sistema
            int quantidade = processos.Length;

            Process processo = Process.GetCurrentProcess();
            long memoria = processo.WorkingSet64; //WorkingSet64 mede a memória física no processo

            //NextValue() retorna uma leitura do contador  
            float usoCPU = cpu.NextValue(); 
            float usoDisco = discoPorcent.NextValue();
            double livreDisco = (double)(total - livre) /total * 100; 

            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
            
            uint usoRam = ram.ObterUsoRam();
            
            Console.WriteLine("\nPORCENTAGEM EM USO" +
                            $"\nCPU: {usoCPU,6:f0}%" +  
                            $"\nRAM: {usoRam}%" +
                            $"\nDISCO: {usoDisco:f0}%"
            );

            Console.WriteLine($"\nCapacidade do disco" +
                            $"\nTOTAL EM USO: {livreDisco:f0}" +
                            $"\nTOTAL DISPONIVEL: {100 - (livreDisco):f0}"
            );

            Console.WriteLine("\nWINDOWS INFO"+
                            $"\nUPTIME: {uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s" +
                            $"\nINICIO DESTA SESSÃO: {inicioSessao}" +
                            $"\nTEMPO DECORRIDO DESTA SESSÃO: {DateTime.Now - inicioSessao:hh\\:mm\\:ss}"
            );

            Console.WriteLine("PROCESSOS"+
                            $"\nTOTAL DE PROCESSOS EM EXECUÇÃO: {quantidade}"+
                            $"\nRAM UTILIZADA PELO APP: {(Double)memoria / 1024.0 / 1024.0:f2}MB"
            );
            
            Thread.Sleep(1000);
        }
    }
}