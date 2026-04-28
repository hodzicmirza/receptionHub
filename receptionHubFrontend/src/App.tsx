import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { AuthProvider } from './context/authContext';
import { ThemeProvider } from './context/ThemeContext';
import './index.css'
import ProtectedRoute from './components/layout/ProtectedRoute';
import LoginPage from './pages/LoginPage';
import NovaRezervacija from './pages/NovaRezervacija.tsx';
import Dashboard from './pages/Dashboard.tsx'
import Profil from './pages/Profil.tsx'
import Recepcioneri from './pages/Recepcioneri.tsx';
import Sobe from './pages/Sobe.tsx'

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 1000 * 60 * 5,
      gcTime: 1000 * 60 * 10,
      retry: 1,
      refetchOnWindowFocus: false,
      refetchOnReconnect: true,
    },
    mutations: {
      retry: 1,
    },
  },
});

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <ThemeProvider>
          <AuthProvider>
            <Routes>
              <Route path="/login" element={<LoginPage />} />
              <Route
                path="/dashboard"
                element={
                  <ProtectedRoute>
                    <Dashboard />
                  </ProtectedRoute>
                }
              />
              <Route path="/" element={<Navigate to="/dashboard" replace />} />
              <Route
                path="/novaRezervacija"
                element={
                  <ProtectedRoute>
                    <NovaRezervacija />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/sobe"
                element={
                  <ProtectedRoute>
                    <Sobe />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/recepcioneri"
                element={
                  <ProtectedRoute>
                    <Recepcioneri />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/profil"
                element={
                  <ProtectedRoute>
                    <Profil />
                  </ProtectedRoute>
                }
              />
            </Routes>
          </AuthProvider>
        </ThemeProvider>
      </BrowserRouter>
      <ReactQueryDevtools initialIsOpen={false} />
    </QueryClientProvider>
  );
}

export default App;
