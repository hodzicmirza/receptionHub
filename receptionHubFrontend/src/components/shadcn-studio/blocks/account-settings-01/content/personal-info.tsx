'use client'

import { useEffect, useRef, useState } from 'react'

import { UploadCloudIcon, TrashIcon, ImageIcon } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue
} from '@/components/ui/select'

const countries = [
  { value: 'india', label: 'Indija', flag: 'https://cdn.shadcnstudio.com/ss-assets/flags/india.png' },
  { value: 'china', label: 'Kina', flag: 'https://cdn.shadcnstudio.com/ss-assets/flags/china.png' },
  { value: 'monaco', label: 'Monako', flag: 'https://cdn.shadcnstudio.com/ss-assets/flags/monaco.png' },
  { value: 'serbia', label: 'Srbija', flag: 'https://cdn.shadcnstudio.com/ss-assets/flags/serbia.png' },
  { value: 'romania', label: 'Rumunija', flag: 'https://cdn.shadcnstudio.com/ss-assets/flags/romania.png' },
  { value: 'mayotte', label: 'Majot', flag: 'https://cdn.shadcnstudio.com/ss-assets/flags/mayotte.png' },
  { value: 'iraq', label: 'Irak', flag: 'https://cdn.shadcnstudio.com/ss-assets/flags/iraq.png' },
  { value: 'syria', label: 'Sirija', flag: 'https://cdn.shadcnstudio.com/ss-assets/flags/syria.png' },
  { value: 'korea', label: 'Koreja', flag: 'https://cdn.shadcnstudio.com/ss-assets/flags/korea.png' },
  { value: 'zimbabwe', label: 'Zimbabve', flag: 'https://cdn.shadcnstudio.com/ss-assets/flags/zimbabwe.png' }
]

const PersonalInfo = () => {
  const inputRef = useRef<HTMLInputElement | null>(null)
  const [file, setFile] = useState<File | null>(null)
  const [preview, setPreview] = useState<string | null>(null)

  useEffect(() => {
    if (!file) {
      const t = window.setTimeout(() => setPreview(null), 0)

      return () => clearTimeout(t)
    }

    const url = URL.createObjectURL(file)

    const t = window.setTimeout(() => setPreview(url), 0)

    return () => {
      clearTimeout(t)
      URL.revokeObjectURL(url)
    }
  }, [file])

  const onSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
    const f = e.target.files?.[0]

    if (!f) return

    if (!f.type.startsWith('image/')) {
      window.alert('Molimo odaberite sliku')
      e.currentTarget.value = ''

      return
    }

    if (f.size > 1024 * 1024) {
      window.alert('Veličina fajla mora biti manja od 1MB')
      e.currentTarget.value = ''

      return
    }

    setFile(f)
  }

  const openPicker = () => inputRef.current?.click()

  const remove = () => {
    setFile(null)
    if (inputRef.current) inputRef.current.value = ''
  }

  return (
    <div className='grid grid-cols-1 gap-10 lg:grid-cols-3'>
      {/* Vertical Tabs List */}
      <div className='flex flex-col space-y-1'>
        <h3 className='font-semibold'>Lični podaci</h3>
        <p className='text-muted-foreground text-sm'>Lični Podaci</p>
      </div>

      {/* Content */}
      <div className='space-y-6 lg:col-span-2'>
        <form className='mx-auto'>
          <div className='mb-6 w-full space-y-2'>
            <Label>Vaša slika</Label>
            <div className='flex items-center gap-4'>
              <div
                role='button'
                tabIndex={0}
                aria-label='Postavite svoj avatar'
                onClick={openPicker}
                onKeyDown={e => {
                  if (e.key === 'Enter' || e.key === ' ') {
                    e.preventDefault()
                    openPicker()
                  }
                }}
                className='flex h-20 w-20 cursor-pointer items-center justify-center overflow-hidden rounded-full border border-dashed hover:opacity-95'
              >
                {preview ? (
                  <img src={preview} alt='pregled avatara' className='h-full w-full object-cover' />
                ) : (
                  <ImageIcon />
                )}
              </div>

              <div className='flex items-center gap-2'>
                <input ref={inputRef} type='file' accept='image/*' className='hidden' onChange={onSelect} />
                <Button type='button' variant='outline' onClick={openPicker} className='flex items-center gap-2'>
                  <UploadCloudIcon />
                  Postavi avatar
                </Button>
                <Button type='button' variant='ghost' onClick={remove} disabled={!file} className='text-destructive'>
                  <TrashIcon />
                </Button>
              </div>
            </div>
            <p className='text-muted-foreground text-sm'>Odaberite fotografiju veličine do 1MB.</p>
          </div>
          <div className='grid grid-cols-1 gap-6 sm:grid-cols-2'>
            <div className='flex flex-col items-start gap-2'>
              <Label htmlFor='multi-step-personal-info-first-name'>Ime</Label>
              <Input id='multi-step-personal-info-first-name' placeholder='John' />
            </div>
            <div className='flex flex-col items-start gap-2'>
              <Label htmlFor='multi-step-personal-info-last-name'>Prezime</Label>
              <Input id='multi-step-personal-info-last-name' placeholder='Doe' />
            </div>
            <div className='flex flex-col items-start gap-2'>
              <Label htmlFor='multi-step-personal-info-mobile'>Mobitel</Label>
              <Input id='multi-step-personal-info-mobile' type='tel' placeholder='+1 (555) 123-4567' />
            </div>
            <div className='flex flex-col items-start gap-2'>
              <Label htmlFor='country'>Država</Label>
              <Select>
                <SelectTrigger
                  id='country'
                  className='[&>span_svg]:text-muted-foreground/80 w-full [&>span]:flex [&>span]:items-center [&>span]:gap-2 [&>span_svg]:shrink-0'
                >
                  <SelectValue placeholder='Odaberite državu' />
                </SelectTrigger>
                <SelectContent className='[&_*[role=option]>span>svg]:text-muted-foreground/80 max-h-100 [&_*[role=option]]:pr-8 [&_*[role=option]]:pl-2 [&_*[role=option]>span]:right-2 [&_*[role=option]>span]:left-auto [&_*[role=option]>span]:flex [&_*[role=option]>span]:items-center [&_*[role=option]>span]:gap-2 [&_*[role=option]>span>svg]:shrink-0'>
                  {countries.map(country => (
                    <SelectItem key={country.value} value={country.value}>
                      <img src={country.flag} alt={`Zastava ${country.label}`} className='h-4 w-5' />{' '}
                      <span className='truncate'>{country.label}</span>
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div className='space-y-2'>
              <Label htmlFor='gender'>Spol</Label>
              <Select>
                <SelectTrigger id='gender' className='w-full'>
                  <SelectValue placeholder='Odaberite spol' />
                </SelectTrigger>
                <SelectContent>
                  <SelectGroup>
                    <SelectItem value='male'>Muški</SelectItem>
                    <SelectItem value='female'>Ženski</SelectItem>
                    <SelectItem value='other'>Ostalo</SelectItem>
                  </SelectGroup>
                </SelectContent>
              </Select>
            </div>

            <div className='space-y-2'>
              <Label htmlFor='role'>Uloga</Label>
              <Select>
                <SelectTrigger id='role' className='w-full'>
                  <SelectValue placeholder='Odaberite ulogu' />
                </SelectTrigger>
                <SelectContent>
                  <SelectGroup>
                    <SelectItem value='admin'>Administrator</SelectItem>
                    <SelectItem value='user'>Korisnik</SelectItem>
                    <SelectItem value='other'>Ostalo</SelectItem>
                  </SelectGroup>
                </SelectContent>
              </Select>
            </div>
          </div>
        </form>
        <div className='flex justify-end'>
          <Button type='submit' className='max-sm:w-full'>
            Sačuvaj promjene
          </Button>
        </div>
      </div>
    </div>
  )
}

export default PersonalInfo
