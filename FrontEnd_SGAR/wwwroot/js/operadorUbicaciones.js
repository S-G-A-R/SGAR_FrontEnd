let socket;
let leafletMapInstance;
let operatorMarkers = {};
let storedDotNetRef = null;
let locationTimeoutId = null;


export function conectarSocket(dotNetHelper) {
    if (socket) {
        console.log("Ya existe un socket, desconectando el viejo.");
        socket.disconnect();
    }

    console.log("Intentando conectar al socket desde JS...");

    socket = io("https://sgar-navigation.onrender.com", {
        transports: ["polling"], 
        EIO: 4                   
    });
    socket.on("connect", () => {
        console.log("¡Socket JS Conectado! ID:", socket.id);
        dotNetHelper.invokeMethodAsync("OnSocketConnected");
    });

    socket.on("nuevaUbicacionOperador", (data) => {
        console.log("JS Recibió 'nuevaUbicacionOperador'", data);
        dotNetHelper.invokeMethodAsync("OnSocketDataReceived", data);
    });

    socket.on("connect_error", (err) => {
        console.error("Error de conexión Socket JS:", err.message);
        dotNetHelper.invokeMethodAsync("OnSocketError", err.message);
    });
}

export function emitirUbicacion(datos) {
    if (socket && socket.connected) {
        socket.emit("actualizarUbicacion", datos);
    } else {
        console.warn("Socket JS no conectado, omitiendo 'emit'.");
    }
}

export const leafletFunctions = {
    actualizarMarcador: (lat, lon, id) => {
        let marker = operatorMarkers[id];

        if (marker) {
            marker.setLatLng([lat, lon]);
        }
        else {
            marker = L.marker([lat, lon]).addTo(leafletMapInstance)
                .bindPopup('Tu ubicación actual.')
                .openPopup();

            operatorMarkers[id] = marker;
        }
    },
    cargarMapa: (lat, lon, idOp) => {
        const mapIcon = document.getElementById('mapIcon');
        const activateBtn = document.getElementById('activateBtn');
        const mapContainer = document.getElementById('leafletMap');

        if (mapIcon) {
            mapIcon.style.display = 'none';
        }
        if (activateBtn) {
            activateBtn.style.display = 'none';
        }

        if (mapContainer) {
            mapContainer.style.display = 'block';
        } else {
            console.error("No se encontró el contenedor del mapa #leafletMap");
            return;
        }

        if (!leafletMapInstance) {
            leafletMapInstance = L.map('leafletMap').setView([lat, lon], 18); 

            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '© OpenStreetMap'
            }).addTo(leafletMapInstance);

            var marker = L.marker([lat, lon]).addTo(leafletMapInstance)
                .bindPopup('Tu ubicación actual.')
                .openPopup();

            operatorMarkers[idOp] = marker;
        } else {
            leafletMapInstance.setView([lat, lon], 16);
        }
    }
};


export function sendUbication(dotNetObjRef, idOp){
    if (storedDotNetRef === null) {
        storedDotNetRef = dotNetObjRef;
        console.log("Referencia de .NET almacenada para futuras llamadas");
    }

    const obtenerUbicacion = () => {

        if (navigator.geolocation) {

            navigator.geolocation.getCurrentPosition(
                (position) => { 

                    var coords = [position.coords.longitude, position.coords.latitude];
                    storedDotNetRef.invokeMethodAsync('SetearUbicacion', coords);

                    if (!leafletMapInstance) {
                        leafletFunctions.cargarMapa(position.coords.latitude, position.coords.longitude, idOp);
                    }
                }
            );
        } else {
            console.error("Geolocalización no es soportada por este navegador.");
        }
    };

    obtenerUbicacion();
    

    if (locationTimeoutId === null) {
        locationTimeoutId = setInterval(obtenerUbicacion, 10000);
        console.log("Timeout configurado para repetir cada 10 segundos");
    }
};

export function stopUbicationTracking(){
    if (locationTimeoutId !== null) {
        clearInterval(locationTimeoutId);
        locationTimeoutId = null;
        console.log("Rastreo de ubicación detenido (timer)");
    }
    if (socket && socket.connected) {
        socket.disconnect();
        console.log("Socket JS desconectado.");
    }

    if (leafletMapInstance) {
        leafletMapInstance.remove();
        leafletMapInstance = null; 
    }

    const mapIcon = document.getElementById('mapIcon');
    const activateBtn = document.getElementById('activateBtn');
    const mapContainer = document.getElementById('leafletMap');

    if (mapIcon) {
        mapIcon.style.display = 'block';
    }
    if (activateBtn) {
        activateBtn.style.display = 'block'; 
    }
    if (mapContainer) {
        mapContainer.style.display = 'none';
    }
};