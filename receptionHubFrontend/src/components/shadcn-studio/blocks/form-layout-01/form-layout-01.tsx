import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'

const FormLayout = () => {
  return (
    <form>
      <div className='mb-8 space-y-2'>
        <h2 className='text-xl font-semibold'>Personal Information</h2>
        <p className='text-muted-foreground'>
          Kreirajte Novu Rezervaciju:
        </p>
      </div>

      <div className='grid grid-cols-1 gap-6 sm:grid-cols-2'>
        <div className='flex flex-col items-start gap-2'>
          <Label htmlFor='multi-step-personal-info-first-name'>First Name</Label>
          <Input id='multi-step-personal-info-first-name' placeholder='John' />
        </div>
        <div className='flex flex-col items-start gap-2'>
          <Label htmlFor='multi-step-personal-info-last-name'>Last Name</Label>
          <Input id='multi-step-personal-info-last-name' placeholder='Doe' />
        </div>
        <div className='flex flex-col items-start gap-2'>
          <Label htmlFor='multi-step-personal-info-mobile'>Mobile</Label>
          <Input id='multi-step-personal-info-mobile' placeholder='+1 (555) 123-4567' />
        </div>
        <div className='flex flex-col items-start gap-2'>
          <Label htmlFor='multi-step-personal-info-pincode'>Pincode</Label>
          <Input id='multi-step-personal-info-pincode' placeholder='Postal Code' />
        </div>
        <div className='flex flex-col items-start gap-2 sm:col-span-2'>
          <Label htmlFor='multi-step-personal-info-address'>Address</Label>
          <Input id='multi-step-personal-info-address' placeholder='123 Main St' />
        </div>
        <div className='flex flex-col items-start gap-2 sm:col-span-2'>
          <Label htmlFor='multi-step-personal-info-landmark'>Landmark</Label>
          <Input id='multi-step-personal-info-landmark' placeholder='Near Central Park, New York' />
        </div>
        <div className='flex flex-col items-start gap-2'>
          <Label htmlFor='multi-step-personal-info-city'>City</Label>
          <Input id='multi-step-personal-info-city' placeholder='New York' />
        </div>
        <div className='flex flex-col items-start gap-2'>
          <Label htmlFor='multi-step-personal-info-state'>State</Label>
          <Input id='multi-step-personal-info-state' placeholder='NY' />
        </div>
      </div>

      <div className='mt-8 flex justify-end'>
        <Button type='submit'>Save Information</Button>
      </div>
    </form>
  )
}

export default FormLayout
