import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import {
  loginApi,
  logoutApi,
  getStoredUser,
  isTokenValid,
  setStoredSession,
  clearStoredSession,
} from '../services/authService.ts';

export const authKeys = {
  user: ['auth', 'user'] as const,
  isAuthenticated: ['auth', 'isAuthenticated'] as const,
};

export const useCurrentUser = () => {
  return useQuery({
    queryKey: authKeys.user,
    queryFn: getStoredUser,
    staleTime: Infinity, // Ne mijenja se osim login/logout
    refetchOnWindowFocus: false,
  });
};

export const useIsAuthenticated = () => {
  return useQuery({
    queryKey: authKeys.isAuthenticated,
    queryFn: isTokenValid,
    staleTime: 5 * 60000,
  });
};

export const useLogin = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ KorisnickoIme, Lozinka }: { KorisnickoIme: string; Lozinka: string }) =>
      loginApi(KorisnickoIme, Lozinka),

    onSuccess: (user) => {
      setStoredSession(user);

      queryClient.setQueryData(authKeys.user, user);
      queryClient.setQueryData(authKeys.isAuthenticated, true);

      queryClient.invalidateQueries({ queryKey: ['reservations'] });
    },

    onError: (error: Error) => {
      console.error('Login failed:', error.message);
      clearStoredSession();
      queryClient.setQueryData(authKeys.user, null);
      queryClient.setQueryData(authKeys.isAuthenticated, false);
    },
  });
};

export const useLogout = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: logoutApi,

    onSuccess: () => {
      clearStoredSession();

      queryClient.setQueryData(authKeys.user, null);
      queryClient.setQueryData(authKeys.isAuthenticated, false);

      queryClient.clear();
    },

    onError: () => {
      clearStoredSession();
      queryClient.setQueryData(authKeys.user, null);
      queryClient.setQueryData(authKeys.isAuthenticated, false);
    },
  });
};
