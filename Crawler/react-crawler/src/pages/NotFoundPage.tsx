/* eslint-disable @typescript-eslint/no-unsafe-call */
import { useEffect, useRef } from "react";

function NotFoundPage() {
  const audioRef = useRef(null);

  useEffect(() => {
    // Sayfa yüklendiğinde sesi otomatik olarak çalmak için
    audioRef.current.play();
  }, []);
  return (
    <>
      <div className="flex justify-center items-center min-h-screen ">
        <div>
          <img src="/gandalf-lol.gif" alt="Crawler Logo" className="mx-auto" />
          <audio ref={audioRef} controls className="mx-auto mt-4">
            <source
              src="/Gandalf_Head_Shake_Epic_Sax(HD).mp3"
              type="audio/mpeg"
            />
            Your browser does not support the audio element.
          </audio>
          <p className="text-5xl font-bold">Ups... wrong page dude</p>
        </div>
      </div>
    </>
  );
}

export default NotFoundPage;
