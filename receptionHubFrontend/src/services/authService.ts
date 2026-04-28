import api from "@/services/api";
import { type LoginDto } from "@/types/dtos/auth.dto";

export interface AuthUser {
  token: string;
  istice: string;
  ime: string;
  prezime: string;
  korisnickoIme: string;
  pozicija: string;
}

export const loginApi = async (KorisnickoIme: string, Lozinka: string): Promise<AuthUser> => {
  const loginData: LoginDto = { korisnickoIme: KorisnickoIme, lozinka: Lozinka };
  const response = await api.post<AuthUser>("/Autentifikacija/login", loginData);
  return response.data;
};

export const logoutApi = async (): Promise<void> => {
  await api.post("/Autentifikacija/logout");
};

export const getStoredUser = (): AuthUser | null => {
  const userIzStorage = localStorage.getItem("user");
  if (!userIzStorage) return null;
  try {
    return JSON.parse(userIzStorage) as AuthUser;
  } catch {
    return null;
  }
};

export const getStoredToken = (): string | null => {
  return localStorage.getItem("token");
};

export const isTokenValid = (): boolean => {
  const token = getStoredToken();
  if (!token) return false;
  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    return payload.exp * 1000 > Date.now();
  } catch {
    return false;
  }
};

export const setStoredSession = (user: AuthUser): void => {
  localStorage.setItem("token", user.token);
  localStorage.setItem("user", JSON.stringify(user));
  api.defaults.headers.common["Authorization"] = `Bearer ${user.token}`;
};

export const clearStoredSession = (): void => {
  localStorage.removeItem("token");
  localStorage.removeItem("user");
  delete (api.defaults.headers.common as any)["Authorization"];
};
