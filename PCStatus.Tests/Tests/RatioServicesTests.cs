using Xunit;
using PCStatus.Services;

public class RatioServiceTests
{
    [Fact]
    public void DiscoOcupado_DeveRetornar70Porcento()
    {
        // Arrange - prepara os valores
        long total = 100;
        long livre = 30;
        
        // Act - executa o metodo
        double resultado = RatioService.discoOcupado(total, livre);
        
        // Assert - verifica se o resultado esta correto
        Assert.Equal(70, resultado);
    }
}