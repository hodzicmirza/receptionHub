import React from "react";
import { LoginForm } from "@/components/login-form"

const LoginPage: React.FC = () => {

  return (
    <div className="flex min-h-svh flex-col items-center justify-center bg-muted p-6 md:p-10">
      <div className="w-full max-w-sm md:max-w-4xl">
        <LoginForm />
      </div>
    </div>
  )
};

export default LoginPage;
