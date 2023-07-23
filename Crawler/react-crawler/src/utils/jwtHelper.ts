/* eslint-disable @typescript-eslint/ban-ts-comment */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import jwtDecode from "jwt-decode";

export function getClaimsFromJwt(token: string) {
  const decodedJwt = jwtDecode(token);

  if (!decodedJwt || typeof decodedJwt !== "object") return {};
  
 // @ts-ignore
  const { uid, email, given_name, family_name, jti } = decodedJwt;

  return { uid, email, given_name, family_name, jti };
}

// jwtHelper.ts dosyasında isTokenExpired fonksiyonunu tanımlayın

export function isTokenExpired(expiration: string): boolean {
  const expirationDate = new Date(expiration).getTime();
  const currentDate = new Date().getTime();

  return expirationDate <= currentDate;
}
