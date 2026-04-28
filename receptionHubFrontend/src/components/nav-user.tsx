import {
  DropdownMenu,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import {
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { useAuth } from "@/context/authContext"
import { useNavigate } from "react-router-dom"
export function NavUser({
  user,
}: {
  user: {
    ime: string;
    prezime: string;
    korisnickoIme?: string;
  } | null  // Može biti null ako korisnik nije ulogovan
}) {
  const { logout } = useAuth()
  const navigate = useNavigate()

  if (!user) {
    return null;
  }

  return (
    <SidebarMenu>
      <SidebarMenuItem>
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <SidebarMenuButton
              size="lg"
              className="data-[state=open]:bg-sidebar-accent data-[state=open]:text-sidebar-accent-foreground"
            >
              <Avatar onClick={e => {
                e.preventDefault()
                navigate('/profil')
              }}
                className="cursor-pointer"
              >
                <AvatarImage src="https://github.com/shadcn.png" />
                <AvatarFallback>CN</AvatarFallback>
              </Avatar>
              <div className="grid flex-1 text-left text-sm leading-tight">
                <span className="truncate font-bold text-md">{user.ime}{" " + user.prezime}</span>

              </div>
              <Button variant="destructive" className="cursor-pointer" onClick={e => {
                e.preventDefault()
                logout();
                navigate('/login');

              }}>
                Odjavi se
              </Button>
            </SidebarMenuButton>
          </DropdownMenuTrigger>
        </DropdownMenu>
      </SidebarMenuItem>
    </SidebarMenu >
  )
}
