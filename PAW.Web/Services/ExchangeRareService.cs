using System.Net;
using System.Text.Json.Serialization;

namespace PAW.Web.Services;

// Contrato del servicio de tipo de cambio.
public interface IExchangeRateService
{
	Task<TipoCambioResponse> GetExchangeRateAsync(
		CancellationToken cancellationToken = default);
}

// Servicio encargado de consultar la API de Hacienda.
public class ExchangeRateService : IExchangeRateService
{
	private readonly HttpClient _httpClient;

	private const string ExchangeRateUrl = "indicadores/tc";

	public ExchangeRateService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<TipoCambioResponse> GetExchangeRateAsync(
		CancellationToken cancellationToken = default)
	{
		using var request = new HttpRequestMessage(
			HttpMethod.Get,
			ExchangeRateUrl);

		request.Version = HttpVersion.Version11;
		request.VersionPolicy =
			HttpVersionPolicy.RequestVersionExact;

		request.Headers.UserAgent.ParseAdd("curl/8.7.1");
		request.Headers.Accept.ParseAdd("*/*");

		using var response = await _httpClient.SendAsync(
			request,
			cancellationToken);

		response.EnsureSuccessStatusCode();

		var result = await response.Content
			.ReadFromJsonAsync<TipoCambioResponse>(
				cancellationToken: cancellationToken);

		return result ?? throw new InvalidOperationException(
			"La API de Hacienda devolvio una respuesta vacia.");
	}
}

// DTO principal de la respuesta de Hacienda.
public class TipoCambioResponse
{
	[JsonPropertyName("dolar")]
	public Dolar Dolar { get; set; } = new();

	[JsonPropertyName("euro")]
	public Euro Euro { get; set; } = new();
}

// Informacion del dolar.
public class Dolar
{
	[JsonPropertyName("venta")]
	public ValorFecha Venta { get; set; } = new();

	[JsonPropertyName("compra")]
	public ValorFecha Compra { get; set; } = new();
}

// Fecha y valor de compra o venta.
public class ValorFecha
{
	[JsonPropertyName("fecha")]
	public DateTime Fecha { get; set; }

	[JsonPropertyName("valor")]
	public decimal Valor { get; set; }
}

// Informacion del euro.
public class Euro
{
	[JsonPropertyName("fecha")]
	public DateTime Fecha { get; set; }

	[JsonPropertyName("dolares")]
	public decimal Dolares { get; set; }

	[JsonPropertyName("colones")]
	public decimal Colones { get; set; }
}