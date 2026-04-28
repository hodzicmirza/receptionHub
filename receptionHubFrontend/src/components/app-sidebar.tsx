import * as React from "react"
import {
  IconInnerShadowTop,
  IconCalendarCheck,
  IconUsersGroup,
  IconBed,
  IconUsers,
  IconHistory,
} from "@tabler/icons-react"

import { NavMain } from "@/components/nav-main"
import { NavUser } from "@/components/nav-user"
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar"
import { useAuth } from "@/context/authContext"

const navMain = [
  {
    title: "Rezervacije",
    url: "/rezervacije",
    icon: IconCalendarCheck,
  },
  {
    title: "Gosti",
    url: "/gosti",
    icon: IconUsersGroup,
  },
  {
    title: "Sobe",
    url: "/sobe",
    icon: IconBed,
  },
  {
    title: "Recepcioneri",
    url: "/recepcioneri",
    icon: IconUsers,
  },
  {
    title: "Logovi",
    url: "/logovi",
    icon: IconHistory,
  },
]

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  const { user } = useAuth();


  return (
    <Sidebar collapsible="offcanvas" {...props}>
      <SidebarHeader>
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton
              asChild
              className="data-[slot=sidebar-menu-button]:p-1.5!"
            >
              <a href="/dashboard">
                <IconInnerShadowTop className="size-5!" />
                <span className="text-base font-semibold">ReceptionHub, Dobro došli!</span>
              </a>
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarHeader>
      <SidebarContent>
        <NavMain items={navMain} />
      </SidebarContent>
      <SidebarFooter>
        <NavUser user={user} />
      </SidebarFooter>
    </Sidebar>
  )
}
