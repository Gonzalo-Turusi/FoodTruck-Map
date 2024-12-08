import requests

def obtener_datos_api():
    url = "https://data.sfgov.org/resource/rqzj-sfat.json"
    
    try:
        response = requests.get(url)
        response.raise_for_status()  # Verifica si hubo errores en la solicitud

        datos = response.json()  # Convierte la respuesta a JSON
        
        if datos:
            print("Datos obtenidos de la API:\n")
            for i, item in enumerate(datos, start=1):
                print(f"Registro {i}:")
                for clave, valor in item.items():
                    print(f"  {clave}: {valor}")
                print("-" * 40)
        else:
            print("No se encontraron datos.")
    
    except requests.exceptions.RequestException as e:
        print(f"Error al realizar la solicitud: {e}")

if __name__ == "__main__":
    obtener_datos_api()
