import { useLogin } from "@/hooks/useAuth"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import { Card, CardContent } from "@/components/ui/card"
import {
  Field,
  FieldDescription,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import { useState } from "react"
import { useNavigate } from "react-router-dom"

export function LoginForm({
  className,
  ...props
}: React.ComponentProps<"div">) {
  const [korisnickoIme, setKorisnickoIme] = useState("");
  const [lozinka, setLozinka] = useState("");
  const [localError, setLocalError] = useState("");

  const login = useLogin()
  const navigate = useNavigate();

  const handleSubmit = async (e: any) => {
    e.preventDefault();
    setLocalError("");

    if (!korisnickoIme.trim() || !lozinka.trim()) {
      setLocalError("Popunite sva polja");
      return;
    }

    try {
      await login.mutateAsync({
        KorisnickoIme: korisnickoIme,
        Lozinka: lozinka
      })

      navigate("/dashboard")
    } catch (err) {
      setLocalError("Prijava nije uspjela");
    }

  };
  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card className="overflow-hidden p-0">
        <CardContent className="grid p-0 md:grid-cols-2">
          <form className="p-6 md:p-8" onSubmit={handleSubmit}>
            <FieldGroup>
              <div className="flex flex-col items-center gap-2 text-center">
                <h1 className="text-2xl font-bold">
                  Dobro došli na ReceptionHub
                </h1>
                <p className="text-balance text-muted-foreground">
                  Pristupite Vašem nalogu.
                </p>
              </div>
              {(localError || login.isError) && (
                <div className="text-sm text-red-500 text-center">
                  {localError || login.error?.message}
                </div>
              )}
              <Field>
                <FieldLabel >Korisničko ime</FieldLabel>
                <Input
                  id="korisnickoIme"
                  type="text"
                  required
                  value={korisnickoIme}
                  onChange={(e) => setKorisnickoIme(e.target.value)}
                  disabled={login.isPending}
                />
              </Field>
              <Field>
                <div className="flex items-center cursor-pointer">
                  <FieldLabel >Lozinka</FieldLabel>
                  <span
                    onClick={e => {
                      e.preventDefault()
                    }}
                    className="ml-auto text-sm underline-offset-2 hover:underline text-blue-600"
                  >
                    Zaboravljena lozinka?
                  </span>
                </div>
                <Input
                  id="lozinka"
                  type="password"
                  required
                  value={lozinka}
                  onChange={(e) => setLozinka(e.target.value)}
                  disabled={login.isPending}
                />
              </Field>
              <Field>
                <Button type="submit"> {login.isPending ? "Logovanje..." : "Login"} </Button>
              </Field>
            </FieldGroup>
          </form>
          <div className="relative hidden bg-muted md:block">
            <img
              src="../../public/ROYAL_LOGO.jpg"
              alt="Image"
              className="absolute inset-0 h-full w-full object-cover "
            />
          </div>
        </CardContent>
      </Card>
      <FieldDescription className="px-6 text-center">
        Sva prava zadržana <a href="https://www.linkedin.com/in/mirza-hodžić-7157aa229">Mirza Hodžić</a>.
      </FieldDescription>
    </div>
  )
}
