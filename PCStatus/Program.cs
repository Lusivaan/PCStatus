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

            //NextValue() retorna uma leitura do contador  
            float usoCPU = cpu.NextValue(); 
            float usoDisco = discoPorcent.NextValue();
            double livreDisco = (double)(total - livre) /total * 100; 

            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
            
            uint usoRam = ram.ObterUsoRam();
            
            Console.WriteLine("PORCENTAGEM EM USO");
            Console.WriteLine($"CPU: {usoCPU:f0}%");
            Console.WriteLine($"RAM: {usoRam}%");
            Console.WriteLine($"DISCO: {usoDisco:f0}%");

            Console.WriteLine($"\nCapacidade do disco");
            Console.WriteLine($"TOTAL EM USO: {livreDisco:f0}");
            Console.WriteLine($"TOTAL DISPONIVEL: {100 - (livreDisco):f0}");

            Console.WriteLine("\nWINDOWS INFO");
            Console.WriteLine($"UPTIME: {uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s");
            Console.WriteLine($"INICIO DESTA SESSÃO: {inicioSessao}");
            Console.WriteLine($"TEMPO DECORRIDO DESTA SESSÃO: {DateTime.Now - inicioSessao:hh\\:mm\\:ss}");

            Thread.Sleep(1000);
        }
    }
}