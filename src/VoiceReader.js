import React, { useRef } from "react";

const VoiceReader = () => {
  // Referencia al elemento que quieres leer
  const textRef = useRef(null);

  // Función para leer el texto
  const speakText = () => {
    const textToRead = textRef.current?.innerText || "";
    if (!textToRead) {
      alert("No hay texto para leer");
      return;
    }

    const utterance = new SpeechSynthesisUtterance(textToRead);
    utterance.lang = "es-MX"; // idioma (puedes cambiar a "es-ES" o "en-US")
    utterance.rate = 1; // velocidad
    utterance.pitch = 3; // tono
    window.speechSynthesis.speak(utterance);
  };

  // Función para detener la lectura
  const stopSpeaking = () => {
    window.speechSynthesis.cancel();
  };

  return (
    <div style={{ padding: 20, textAlign: "center" }}>
      <h2>🗣️ Lector de texto en pantalla</h2>

      <p ref={textRef}>
        Hola, soy un párrafo en pantalla que el sistema leerá en voz alta cuando presiones el botón.
      </p>

      <div style={{ marginTop: 20 }}>
        <button
          onClick={speakText}
          style={{
            padding: "10px 20px",
            background: "#007bff",
            color: "white",
            border: "none",
            borderRadius: "8px",
            cursor: "pointer",
            marginRight: "10px"
          }}
        >
          🔊 Leer texto
        </button>

        <button
          onClick={stopSpeaking}
          style={{
            padding: "10px 20px",
            background: "#dc3545",
            color: "white",
            border: "none",
            borderRadius: "8px",
            cursor: "pointer"
          }}
        >
          ⏹️ Detener
        </button>
      </div>
    </div>
  );
};

export default VoiceReader;
