//ESTA CLASE TIENE CASI TODA LA LOGICA DE API PARA EL TIPO DE CAMBIO 

async function loadExchangeRate() {

    const cacheKey = "exchangeRateData";
    const cacheTimeKey = "exchangeRateTime";

    const cachedData = localStorage.getItem(cacheKey);
    const cachedTime = localStorage.getItem(cacheTimeKey);

    const now = new Date().getTime();
    
    if (cachedData && cachedTime &&   /// ESTO ES Q SI HAY datos y tienen menos de una hora, no vuelva a consultar Hacienda
        now - cachedTime < 3600000) {

        const data = JSON.parse(cachedData); //convierte ese texto nuevamente en un objeto JavaScript.

        showExchangeRate(data); ///esta función coloca los valores en la pantalla.
        return;
    }

    const response = await fetch(   //fetch() sirve para realizar una solicitud HTTP desde JavaScript.
        "https://api.hacienda.go.cr/indicadores/tc"  //este es el link de dnd jala los valores de dolar y euro ENDPOINT
    );

    const data = await response.json(); //ESTO se encarga de  convertir la respuesta a JSON

    localStorage.setItem(  //aqui guarde los datos en cache
        cacheKey,
        JSON.stringify(data)
    );

    localStorage.setItem(   //aqu i guarda la hora 
        cacheTimeKey,
        now
    );

    showExchangeRate(data);
}


function showExchangeRate(data) {  //tomar los datos y mostrarlos en el HTML

    document.getElementById("dolarCompra").innerText =
        data.dolar.compra.valor;

    document.getElementById("dolarVenta").innerText =
        data.dolar.venta.valor;

    document.getElementById("euro").innerText =
        data.euro.colones;
}

document.addEventListener("DOMContentLoaded", function () {   //esta funcion hace : “Cuando el HTML haya terminado de cargar, ejecute loadExchangeRate().”
    loadExchangeRate();
});