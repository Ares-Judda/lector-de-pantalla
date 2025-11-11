import React from "react";
import { useSpeechSynthesis } from "react-speech-kit";

const VoiceReader = () => {
  const { speak, cancel, speaking } = useSpeechSynthesis();
  const text = "Hola, este texto se está leyendo usando react-speech-kit.";

  return (
    <div style={{ padding: 20, textAlign: "center" }}>
      <h2>🗣️ Lector de Texto en React</h2>
      <p>{text}</p>

      <button
        onClick={() => speak({ text, lang: "es-MX" })}
        style={{
          padding: "10px 20px",
          fontSize: "16px",
          backgroundColor: "#007bff",
          color: "#fff",
          border: "none",
          borderRadius: "8px",
          cursor: "pointer",
          marginRight: "10px"
        }}
      >
        🔊 Leer texto
      </button>

      {speaking && (
        <button
          onClick={cancel}
          style={{
            padding: "10px 20px",
            fontSize: "16px",
            backgroundColor: "#dc3545",
            color: "#fff",
            border: "none",
            borderRadius: "8px",
            cursor: "pointer"
          }}
        >
          ⏹️ Detener
        </button>
      )}
    </div>
  );
};

export default VoiceReader;
