import { useMemo, useState } from 'react'

import { CheckIcon, MailIcon, XIcon, EyeIcon, EyeOffIcon } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'

import { cn } from '@/lib/utils'

const requirements = [
  { regex: /.{12,}/, text: 'Najmanje 12 karaktera' },
  { regex: /[a-z]/, text: 'Najmanje 1 malo slovo' },
  { regex: /[A-Z]/, text: 'Najmanje 1 veliko slovo' },
  { regex: /[0-9]/, text: 'Najmanje 1 broj' },
  {
    regex: /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>/?]/,
    text: 'Najmanje 1 specijalni karakter'
  }
]

const EmailPass = () => {
  const [isVisible, setIsVisible] = useState(false)

  const [password, setPassword] = useState('')

  const toggleVisibility = () => setIsVisible(prevState => !prevState)

  const strength = requirements.map(req => ({
    met: req.regex.test(password),
    text: req.text
  }))

  const strengthScore = useMemo(() => {
    return strength.filter(req => req.met).length
  }, [strength])

  const getColor = (score: number) => {
    if (score === 0) return 'bg-border'
    if (score <= 1) return 'bg-destructive'
    if (score <= 2) return 'bg-orange-500 '
    if (score <= 3) return 'bg-amber-500'
    if (score === 4) return 'bg-yellow-400'

    return 'bg-green-500'
  }

  const getText = (score: number) => {
    if (score === 0) return 'Unesite lozinku'
    if (score <= 2) return 'Slaba lozinka'
    if (score <= 3) return 'Srednje jaka lozinka'
    if (score === 4) return 'Jaka lozinka'

    return 'Veoma jaka lozinka'
  }

  return (
    <div className='grid grid-cols-1 gap-10 lg:grid-cols-3'>
      {/* Vertical Tabs List */}
      <div className='flex flex-col space-y-1'>
        <h3 className='font-semibold'>Email & Lozinka</h3>
        <p className='text-muted-foreground text-sm'>Upravljajte postavkama emaila i lozinke.</p>
      </div>

      {/* Content */}
      <div className='lg:col-span-2'>
        <form className='mx-auto space-y-6'>
          <div className='w-full space-y-2'>
            <Label htmlFor='email' className='gap-1'>
              Email<span className='text-destructive'>*</span>
            </Label>
            <div className='relative'>
              <Input id='email' type='email' placeholder='Email adresa' className='peer pr-9' required />
              <div className='text-muted-foreground pointer-events-none absolute inset-y-0 right-0 flex items-center justify-center pr-3 peer-disabled:opacity-50'>
                <MailIcon className='size-4' />
                <span className='sr-only'>Email</span>
              </div>
            </div>
          </div>
          <div className='w-full space-y-2'>
            <Label htmlFor='current-password' className='gap-1'>
              Trenutna lozinka<span className='text-destructive'>*</span>
            </Label>
            <div className='relative'>
              <Input
                id='current-password'
                type={isVisible ? 'text' : 'password'}
                placeholder='Lozinka'
                className='pr-9'
                required
              />
              <Button
                variant='ghost'
                size='icon'
                onClick={() => setIsVisible(prevState => !prevState)}
                className='text-muted-foreground focus-visible:ring-ring/50 absolute inset-y-0 right-0 rounded-l-none hover:bg-transparent'
              >
                {isVisible ? <EyeOffIcon /> : <EyeIcon />}
                <span className='sr-only'>{isVisible ? 'Sakrij lozinku' : 'Prikaži lozinku'}</span>
              </Button>
            </div>
          </div>
          <div className='w-full space-y-2'>
            <Label htmlFor='new-password' className='gap-1'>
              Nova lozinka<span className='text-destructive'>*</span>
            </Label>
            <div className='relative mb-3'>
              <Input
                id='new-password'
                type={isVisible ? 'text' : 'password'}
                placeholder='Lozinka'
                value={password}
                onChange={e => setPassword(e.target.value)}
                className='pr-9'
                required
              />
              <Button
                variant='ghost'
                size='icon'
                onClick={toggleVisibility}
                className='text-muted-foreground focus-visible:ring-ring/50 absolute inset-y-0 right-0 rounded-l-none hover:bg-transparent'
              >
                {isVisible ? <EyeOffIcon /> : <EyeIcon />}
                <span className='sr-only'>{isVisible ? 'Sakrij lozinku' : 'Prikaži lozinku'}</span>
              </Button>
            </div>

            <div className='mb-4 flex h-1 w-full gap-1'>
              {Array.from({ length: 5 }).map((_, index) => (
                <span
                  key={index}
                  className={cn(
                    'h-full flex-1 rounded-full transition-all duration-500 ease-out',
                    index < strengthScore ? getColor(strengthScore) : 'bg-border'
                  )}
                />
              ))}
            </div>

            <p className='text-foreground text-sm font-medium'>{getText(strengthScore)}. Mora sadržavati:</p>

            <ul className='mb-4 space-y-1.5'>
              {strength.map((req, index) => (
                <li key={index} className='flex items-center gap-2'>
                  {req.met ? (
                    <CheckIcon className='size-4 text-green-600 dark:text-green-400' />
                  ) : (
                    <XIcon className='text-muted-foreground size-4' />
                  )}
                  <span
                    className={cn('text-xs', req.met ? 'text-green-600 dark:text-green-400' : 'text-muted-foreground')}
                  >
                    {req.text}
                    <span className='sr-only'>{req.met ? ' - Uslov ispunjen' : ' - Uslov nije ispunjen'}</span>
                  </span>
                </li>
              ))}
            </ul>
          </div>

          <div className='mt-6 flex justify-end'>
            <Button type='submit' className='max-sm:w-full'>
              Sačuvaj promjene
            </Button>
          </div>
        </form>
      </div>
    </div>
  )
}

export default EmailPass
