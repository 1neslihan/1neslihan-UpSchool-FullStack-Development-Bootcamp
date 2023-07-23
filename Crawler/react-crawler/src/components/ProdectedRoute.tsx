/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-return */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import React from "react";

import { Navigate } from "react-router-dom";

type ProtectedRouteProps = {
  children: React.ReactElement;
};


// Kullanıcı oturum bilgisini kontrol etmek için fonksiyon
function checkUserAuthentication() {
  const localJwt = JSON.parse(localStorage.getItem("crawler_user"));
  // Eğer localJwt değeri varsa ve accessToken alanı doluysa kullanıcı giriş yapmış kabul ediyoruz.
  return localJwt && localJwt.accessToken;
}

export default function ProtectedRoute({ children }: ProtectedRouteProps) {
  
  const isUserAuthenticated = checkUserAuthentication();
  
 

  if (!isUserAuthenticated) return <Navigate to="/homepage" />;
  

  return children;
}


