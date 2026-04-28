import { useState, Fragment } from 'react';
import { useAuth } from '../../context/authContext';
import { useNavigate, Link, useLocation } from 'react-router-dom';
import { Dialog, Transition } from '@headlessui/react';
import {
  Sun,
  Moon,
  Home,
  Users,
  FileText,
  Settings,
  Calendar,
  Building2,
  LogOut,
  ChevronRight,
  UserPlus,
  Menu as MenuIcon,
  X
} from 'lucide-react';
import { TipPozicije } from '../../models/TipPozicije';
import { useTheme } from '../../context/ThemeContext';

interface NavItem {
  name: string;
  href: string;
  icon: React.ElementType;
  roles?: TipPozicije[];
}

const navigation: NavItem[] = [
  { name: 'Dashboard', href: '/dashboard', icon: Home, roles: [TipPozicije.Admin, TipPozicije.Recepcioner] },
  { name: 'Recepcioneri', href: '/recepcioneri', icon: Users, roles: [TipPozicije.Admin] },
  { name: 'Sobe', href: '/sobe', icon: Building2, roles: [TipPozicije.Admin, TipPozicije.Recepcioner] },
  { name: 'Gosti', href: '/gosti', icon: UserPlus, roles: [TipPozicije.Admin, TipPozicije.Recepcioner] },
  { name: 'Rezervacije', href: '/rezervacije', icon: Calendar, roles: [TipPozicije.Admin, TipPozicije.Recepcioner] },
  { name: 'Logovi', href: '/logovi', icon: FileText, roles: [TipPozicije.Admin] },
  { name: 'Postavke', href: '/postavke', icon: Settings, roles: [TipPozicije.Admin] },
];

function classNames(...classes: string[]) {
  return classes.filter(Boolean).join(' ');
}

const getPozicijaNaziv = (pozicija: TipPozicije): string => {
  switch (pozicija) {
    case TipPozicije.Admin:
      return 'Administrator';
    case TipPozicije.Recepcioner:
      return 'Recepcioner';
    default:
      return 'Nepoznato';
  }
};

export default function NavBar() {
  const { theme, toggleTheme } = useTheme();
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const filteredNavigation = navigation.filter(item =>
    !item.roles || (user?.pozicija && item.roles.includes(user.pozicija as TipPozicije))
  );

  return (
    <>
      {/* Desktop sidebar */}
      <div className="hidden lg:fixed lg:inset-y-0 lg:z-50 lg:flex lg:w-72 lg:flex-col">
        <div className="flex grow flex-col gap-y-5 overflow-y-auto bg-white dark:bg-gray-900 px-6 pb-4 border-r border-gray-200 dark:border-gray-800">
          <div className="flex h-16 shrink-0 items-center justify-between">
            <h1 className="text-2xl font-bold text-blue-600 dark:text-blue-400">ReceptionHub</h1>

            {/* Theme toggle - desktop */}
            <button
              onClick={toggleTheme}
              className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
              aria-label="Promijeni temu"
            >
              {theme === 'light' ? (
                <Moon className="h-5 w-5 text-gray-600 dark:text-gray-400" />
              ) : (
                <Sun className="h-5 w-5 text-yellow-500" />
              )}
            </button>
          </div>

          <Link
            to="/profil"
            className="flex items-center gap-x-4 py-3 px-2 bg-gray-50 dark:bg-gray-800 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 transition-all duration-200 cursor-pointer group"
          >
            <div className="h-10 w-10 rounded-full bg-blue-500 flex items-center justify-center text-white font-semibold group-hover:bg-blue-600 transition-colors">
              {user?.ime?.charAt(0)}{user?.prezime?.charAt(0)}
            </div>
            <div className="flex-1 min-w-0">
              <p className="text-sm font-semibold text-gray-900 dark:text-white truncate group-hover:text-blue-600 dark:group-hover:text-blue-400 transition-colors">
                {user?.ime} {user?.prezime}
              </p>
              <p className="text-xs text-gray-500 dark:text-gray-400 truncate">
                {user?.pozicija ? getPozicijaNaziv(user.pozicija as TipPozicije) : ''}
              </p>
            </div>

            <ChevronRight className="h-4 w-4 text-gray-400 group-hover:text-blue-500 dark:group-hover:text-blue-400 opacity-0 group-hover:opacity-100 transition-all" />
          </Link>

          <nav className="flex flex-1 flex-col">
            <ul role="list" className="flex flex-1 flex-col gap-y-7">
              <li>
                <ul role="list" className="-mx-2 space-y-1">
                  {filteredNavigation.map((item) => (
                    <li key={item.name}>
                      <Link
                        to={item.href}
                        className={classNames(
                          location.pathname === item.href
                            ? 'bg-gray-50 dark:bg-gray-800 text-blue-600 dark:text-blue-400'
                            : 'text-gray-700 dark:text-gray-300 hover:text-blue-600 dark:hover:text-blue-400 hover:bg-gray-50 dark:hover:bg-gray-800',
                          'group flex gap-x-3 rounded-md p-2 text-sm leading-6 font-semibold'
                        )}
                      >
                        <item.icon
                          className={classNames(
                            location.pathname === item.href ? 'text-blue-600 dark:text-blue-400' : 'text-gray-400 dark:text-gray-500 group-hover:text-blue-600 dark:group-hover:text-blue-400',
                            'h-6 w-6 shrink-0'
                          )}
                          aria-hidden="true"
                        />
                        {item.name}
                      </Link>
                    </li>
                  ))}
                </ul>
              </li>

              <li className="mt-auto">
                <button
                  onClick={handleLogout}
                  className="flex w-full items-center gap-x-3 rounded-md p-2 text-sm font-semibold text-red-600 dark:text-red-400 hover:bg-red-50 dark:hover:bg-red-900/20"
                >
                  <LogOut className="h-6 w-6" aria-hidden="true" />
                  Odjavi se
                </button>
              </li>
            </ul>
          </nav>
        </div>
      </div>

      {/* Mobile header */}
      <div className="lg:hidden">
        <div className="fixed top-0 left-0 right-0 z-40 flex h-16 items-center justify-between bg-white dark:bg-gray-900 px-4 shadow-sm border-b border-gray-200 dark:border-gray-800">
          <button
            type="button"
            className="inline-flex items-center justify-center rounded-md p-2 text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-800"
            onClick={() => setMobileMenuOpen(true)}
          >
            <MenuIcon className="h-6 w-6" aria-hidden="true" />
          </button>

          <h1 className="text-xl font-bold text-blue-600 dark:text-blue-400">ReceptionHub</h1>

          <div className="flex items-center gap-2">
            <button
              onClick={toggleTheme}
              className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
            >
              {theme === 'light' ? (
                <Moon className="h-5 w-5 text-gray-600 dark:text-gray-400" />
              ) : (
                <Sun className="h-5 w-5 text-yellow-500" />
              )}
            </button>

          </div>
        </div>

        <Transition show={mobileMenuOpen} as={Fragment}>
          <Dialog as="div" className="relative z-50 lg:hidden" onClose={() => setMobileMenuOpen(false)}>
            <Transition.Child
              as={Fragment}
              enter="transition-opacity ease-linear duration-300"
              enterFrom="opacity-0"
              enterTo="opacity-100"
              leave="transition-opacity ease-linear duration-300"
              leaveFrom="opacity-100"
              leaveTo="opacity-0"
            >
              <div className="fixed inset-0 bg-gray-900/80 dark:bg-gray-950/90" />
            </Transition.Child>

            <div className="fixed inset-0 flex">
              <Transition.Child
                as={Fragment}
                enter="transition ease-in-out duration-300 transform"
                enterFrom="-translate-x-full"
                enterTo="translate-x-0"
                leave="transition ease-in-out duration-300 transform"
                leaveFrom="translate-x-0"
                leaveTo="-translate-x-full"
              >
                <Dialog.Panel className="relative mr-16 flex w-full max-w-xs flex-1">
                  <Transition.Child
                    as={Fragment}
                    enter="ease-in-out duration-300"
                    enterFrom="opacity-0"
                    enterTo="opacity-100"
                    leave="ease-in-out duration-300"
                    leaveFrom="opacity-100"
                    leaveTo="opacity-0"
                  >
                    <div className="absolute left-full top-0 flex w-16 justify-center pt-5">
                      <button
                        type="button"
                        className="-m-2.5 p-2.5"
                        onClick={() => setMobileMenuOpen(false)}
                      >
                        <span className="sr-only">Close sidebar</span>
                        <X className="h-6 w-6 text-white" aria-hidden="true" />
                      </button>
                    </div>
                  </Transition.Child>

                  <div className="flex grow flex-col gap-y-5 overflow-y-auto bg-white dark:bg-gray-900 px-6 pb-4">
                    <div className="flex h-16 shrink-0 items-center">
                      <h1 className="text-xl font-bold text-blue-600 dark:text-blue-400">ReceptionHub</h1>
                    </div>

                    <Link
                      to="/profil"
                      onClick={() => setMobileMenuOpen(false)}
                      className="flex items-center gap-x-4 py-3 px-2 bg-gray-50 dark:bg-gray-800 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 transition-all duration-200 cursor-pointer group"
                    >
                      <div className="h-10 w-10 rounded-full bg-blue-500 flex items-center justify-center text-white font-semibold group-hover:bg-blue-600 transition-colors">
                        {user?.ime?.charAt(0)}{user?.prezime?.charAt(0)}
                      </div>
                      <div className="flex-1 min-w-0">
                        <p className="text-sm font-semibold text-gray-900 dark:text-white truncate group-hover:text-blue-600 dark:group-hover:text-blue-400 transition-colors">
                          {user?.ime} {user?.prezime}
                        </p>
                        <p className="text-xs text-gray-500 dark:text-gray-400 truncate">
                          {user?.pozicija ? getPozicijaNaziv(user.pozicija as TipPozicije) : ''}
                        </p>
                      </div>
                      <ChevronRight className="h-4 w-4 text-gray-400 group-hover:text-blue-500 dark:group-hover:text-blue-400" />
                    </Link>

                    <nav className="flex flex-1 flex-col">
                      <ul role="list" className="flex flex-1 flex-col gap-y-7">
                        <li>
                          <ul role="list" className="-mx-2 space-y-1">
                            {filteredNavigation.map((item) => (
                              <li key={item.name}>
                                <Link
                                  to={item.href}
                                  onClick={() => setMobileMenuOpen(false)}
                                  className={classNames(
                                    location.pathname === item.href
                                      ? 'bg-gray-50 dark:bg-gray-800 text-blue-600 dark:text-blue-400'
                                      : 'text-gray-700 dark:text-gray-300 hover:text-blue-600 dark:hover:text-blue-400 hover:bg-gray-50 dark:hover:bg-gray-800',
                                    'group flex gap-x-3 rounded-md p-2 text-sm leading-6 font-semibold'
                                  )}
                                >
                                  <item.icon
                                    className={classNames(
                                      location.pathname === item.href ? 'text-blue-600 dark:text-blue-400' : 'text-gray-400 dark:text-gray-500 group-hover:text-blue-600 dark:group-hover:text-blue-400',
                                      'h-6 w-6 shrink-0'
                                    )}
                                    aria-hidden="true"
                                  />
                                  {item.name}
                                </Link>
                              </li>
                            ))}
                          </ul>
                        </li>

                        <li className="mt-auto">
                          <button
                            onClick={() => {
                              setMobileMenuOpen(false);
                              handleLogout();
                            }}
                            className="flex w-full items-center gap-x-3 rounded-md p-2 text-sm font-semibold text-red-600 dark:text-red-400 hover:bg-red-50 dark:hover:bg-red-900/20"
                          >
                            <LogOut className="h-6 w-6" aria-hidden="true" />
                            Odjavi se
                          </button>
                        </li>
                      </ul>
                    </nav>
                  </div>
                </Dialog.Panel>
              </Transition.Child>
            </div>
          </Dialog>
        </Transition>
      </div>

      <div className="lg:pl-72 pt-16 lg:pt-0">
        {/* Ovdje će ići children (ostali sadržaj stranice) */}
      </div>
    </>
  );
}
