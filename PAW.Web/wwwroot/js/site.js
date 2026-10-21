"use strict";

// URL del endpoint creado en PAW.Web.
const EXCHANGE_RATE_URL = "/api/exchange-rate";

// Tiempo de cache en el navegador: 12 horas.
const EXCHANGE_RATE_CACHE_KEY = "paw_exchange_rate";
const EXCHANGE_RATE_CACHE_HOURS = 12;

// Consulta el tipo de cambio.
// Primero intenta obtener los datos desde la cache.
async function getExchangeRate() {
    const cached = sessionStorage.getItem(
        EXCHANGE_RATE_CACHE_KEY
    );

    if (cached) {
        try {
            const parsed = JSON.parse(cached);

            if (
                parsed.expiresAt > Date.now() &&
                parsed.data
            ) {
                return parsed.data;
            }
        } catch (error) {
            console.warn(
                "No se pudo leer la cache:",
                error
            );
        }

        sessionStorage.removeItem(
            EXCHANGE_RATE_CACHE_KEY
        );
    }

    // Consulta mediante JavaScript Fetch API.
    const response = await fetch(EXCHANGE_RATE_URL, {
        method: "GET",
        headers: {
            "Accept": "application/json"
        }
    });

    if (!response.ok) {
        throw new Error(
            "Error HTTP: " + response.status
        );
    }

    const data = await response.json();

    // Guardamos la respuesta temporalmente.
    const expiresAt = Date.now() +
        EXCHANGE_RATE_CACHE_HOURS * 60 * 60 * 1000;

    try {
        sessionStorage.setItem(
            EXCHANGE_RATE_CACHE_KEY,
            JSON.stringify({
                data: data,
                expiresAt: expiresAt
            })
        );
    } catch (error) {
        console.warn(
            "No se pudo guardar la cache:",
            error
        );
    }

    return data;
}

// Formatea valores numericos con dos decimales.
function formatCurrency(value, currency) {
    const number = Number(value);

    if (!Number.isFinite(number)) {
        return "No disponible";
    }

    return new Intl.NumberFormat("es-CR", {
        style: "currency",
        currency: currency,
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    }).format(number);
}

// Crea una fila del componente visual.
function createExchangeRateRow(label, value) {
    const row = document.createElement("div");
    row.className = "exchange-rate-row";

    const name = document.createElement("span");
    name.className = "exchange-rate-label";
    name.textContent = label;

    const amount = document.createElement("strong");
    amount.className = "exchange-rate-value";
    amount.textContent = value;

    row.appendChild(name);
    row.appendChild(amount);

    return row;
}

// Carga los datos dentro de la tarjeta.
async function loadExchangeRate() {
    const container = document.getElementById(
        "exchange-rate-content"
    );

    const dateContainer = document.getElementById(
        "exchange-rate-date"
    );

    if (!container) {
        return;
    }

    try {
        const data = await getExchangeRate();

        container.replaceChildren();

        container.appendChild(
            createExchangeRateRow(
                "USD compra",
                formatCurrency(data.dolar.compra.valor, "CRC")
            )
        );

        container.appendChild(
            createExchangeRateRow(
                "USD venta",
                formatCurrency(data.dolar.venta.valor, "CRC")
            )
        );

        container.appendChild(
            createExchangeRateRow(
                "EUR / CRC",
                formatCurrency(data.euro.colones, "CRC")
            )
        );

        container.appendChild(
            createExchangeRateRow(
                "EUR / USD",
                formatCurrency(data.euro.dolares, "USD")
            )
        );

        if (dateContainer && data.dolar.venta.fecha) {
            const date = new Date(
                data.dolar.venta.fecha
            );

            dateContainer.textContent =
                "Fecha de referencia: " +
                date.toLocaleDateString("es-CR", {
                    year: "numeric",
                    month: "long",
                    day: "numeric",
                    timeZone: "UTC"
                });
        }

    } catch (error) {
        console.error(
            "Error al cargar el tipo de cambio:",
            error
        );

        container.textContent =
            "Tipo de cambio no disponible.";

        if (dateContainer) {
            dateContainer.textContent = "";
        }
    }
}

// Ejecutar cuando el documento termine de cargar.
document.addEventListener(
    "DOMContentLoaded",
    loadExchangeRate
);