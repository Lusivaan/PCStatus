using System.Diagnostics; //permite consultar contadores de desempenho do Windows, como CPU, memória e disco.
using PCStatus.Services;

internal static class Program
{
    static void Main()
    {
        Console.Clear();

        bool continuar = true;

        Console.CancelKeyPress += (sender, e) => //evento disparado quando o usuário pressiona Ctrl+C
        {
            e.Cancel= true;
            Volatile.Write(ref continuar, false);
        };

        using PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        using PerformanceCounter discoPorcent = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
        
        DriveInfo disco = new DriveInfo("C");

        MemoryService ram = new MemoryService();

        using Process explorer = Process.GetProcessesByName("explorer").First();
        DateTime inicioSessao = explorer.StartTime;  
        
        
        
        while (Volatile.Read(ref continuar)) //Volatile ajuda as duas threads a enxergarem corretamente o valor de continuar
        {   
            
            Console.SetCursorPosition(0, 0); //move o cursor para a primeira coluna e primeira linha

            Process[] processos = Process.GetProcesses(); // retorna os processos em execução no sistema
            int quantidade = processos.Length;
            foreach(Process p in processos) p.Dispose();

            using Process processo = Process.GetCurrentProcess();
            long memoria = processo.WorkingSet64; //WorkingSet64 mede a memória física no processo
            

            //NextValue() retorna uma leitura do contador  
            float usoCPU = cpu.NextValue(); 
            float usoDisco = discoPorcent.NextValue();

            long totalDisk = disco.TotalSize;
            long livreDisk = disco.TotalFreeSpace;
            
            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
            
            uint usoRam = ram.ObterUsoRam();
            
            Console.WriteLine("\nPORCENTAGEM EM USO" +
                            $"\nCPU: {usoCPU,6:f0}%" +  
                            $"\nRAM: {usoRam}%" +
                            $"\nDISCO: {usoDisco:f0}%"
            );

            Console.WriteLine($"\nCapacidade do disco" +
                            $"\nTOTAL EM USO: {RatioService.discoOcupado(totalDisk, livreDisk):f0} GB" +
                            $"\nTOTAL DISPONIVEL: {RatioService.discoLivre(livreDisk):f0} GB"
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