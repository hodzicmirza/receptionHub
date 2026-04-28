import React, { createContext, useContext, useState, useEffect } from 'react';
// ← PROMIJENA 1: Izbriši ovaj import (ne treba default)
// import authService from '../services/authService.ts';
// ← PROMIJENA 1: Dodaj ove named importe
import {
  getStoredUser,
  loginApi,
  logoutApi,
  isTokenValid
} from '../services/authService';

import type { LoginResponseDto } from '@/types/dtos/auth.dto';

export interface AuthUser {
  token: string;
  istice: string;
  ime: string;
  prezime: string;
  korisnickoIme: string;
  pozicija: string;
}

interface AuthContextType {
  user: AuthUser | null;
  loading: boolean;
  error: string | null;
  login: (korisnickoIme: string, lozinka: string) => Promise<{
    success: boolean;
    data?: LoginResponseDto;
    error?: string;
  }>;
  logout: () => Promise<void>;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<AuthUser | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadUser = () => {
      try {
        // ← PROMIJENA 2: authService.getCurrentUser() → getStoredUser()
        const currentUser = getStoredUser();
        setUser(currentUser);
      } catch (err) {
        console.error('Greška pri učitavanju korisnika:', err);
      } finally {
        setLoading(false);
      }
    };

    loadUser();
  }, []);

  const login = async (korisnickoIme: string, lozinka: string) => {
    try {
      setLoading(true);
      setError(null);

      // ← PROMIJENA 3: authService.login() → loginApi()
      const response = await loginApi(korisnickoIme, lozinka);

      // Spremi u localStorage (dodaj ovo!)
      localStorage.setItem("token", response.token);
      localStorage.setItem("user", JSON.stringify(response));

      setUser(response);

      return { success: true, data: response };
    } catch (err: any) {
      const errorMessage = err.response?.data?.poruka ||
        err.message ||
        'Došlo je do greške prilikom prijave';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  const logout = async () => {
    try {
      // ← PROMIJENA 4: authService.logout() → logoutApi()
      await logoutApi();
    } catch (err) {
      console.error('Greška pri odjavi:', err);
    } finally {
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      setUser(null);
    }
  };

  // ← PROMIJENA 5: authService.isAuthenticated() → isTokenValid()
  const isAuthenticated = !!user && isTokenValid();

  const value: AuthContextType = {
    user,
    loading,
    error,
    login,
    logout,
    isAuthenticated,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
};
