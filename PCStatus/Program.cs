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

        //"Processor" - categoria a monitorar.
        //"% Processor Time" - indicador de utilização do processador.
        //"_Total" - considera todos os processadores lógicos juntos.
        PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        MemoryService ram = new MemoryService();

        while (continuar)
        {   
            //NextValue() retorna uma leitura do contador  
            float usoCPU = cpu.NextValue(); 

            
            uint usoRam = ram.ObterUsoRam();
            
            Console.WriteLine("PORCENTAGEM DE USO");
            Console.WriteLine($"CPU: {usoCPU:f0}%");
            Console.WriteLine($"RAM: {usoRam}%");

            Thread.Sleep(1000);
        }

    }
}