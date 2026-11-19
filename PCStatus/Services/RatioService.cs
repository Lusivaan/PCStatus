namespace PCStatus.Services;

public class RatioService
{
    public static double discoOcupado(long totalDisk, long livreDisk)
    {
        double disco_Ocupado = (double)(totalDisk - livreDisk) /totalDisk * 100;
        return disco_Ocupado;
    }

    public static double discoLivre(long livreDisk)
    {
        double disco_livre = livreDisk /1024.0/ 1024.0 / 1024.0;
        return disco_livre;
    }
    
}