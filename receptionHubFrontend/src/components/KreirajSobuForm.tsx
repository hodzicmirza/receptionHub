// 📁 components/KreirajSobuForm.tsx
import { useState } from 'react';
import { useKreirajSobu } from '@/hooks/useSobe';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { Switch } from '@/components/ui/switch';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { toast } from 'sonner';
import { TipSobe, StatusSobe, type KreirajSobuDto } from '@/types/dtos/soba.dto';

interface KreirajSobuFormProps {
  onSuccess?: () => void;
}

export function KreirajSobuForm({ onSuccess }: KreirajSobuFormProps) {
  const [formData, setFormData] = useState<Partial<KreirajSobuDto>>({
    brojSobe: '',
    tipSobe: TipSobe.Standard,
    maksimalnoGostiju: 2,
    brojKreveta: 1,
    cijenaPoNociBAM: 50,
    imaWiFi: true,
    imaTv: true,
    imaKlimu: true,
    imaKupatilo: true,
  });

  const kreirajSobu = useKreirajSobu();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.brojSobe) {
      toast.error('Molimo unesite broj sobe');
      return;
    }

    try {
      await kreirajSobu.mutateAsync(formData as KreirajSobuDto);
      toast.success(`Soba ${formData.brojSobe} je uspješno kreirana`);
      onSuccess?.();
    } catch (error) {
      toast.error('Greška pri kreiranju sobe');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div className="grid grid-cols-2 gap-4">
        <div>
          <Label>Broj sobe *</Label>
          <Input
            value={formData.brojSobe}
            onChange={(e) => setFormData({ ...formData, brojSobe: e.target.value })}
            placeholder="101"
            className="border-blue-200 focus:ring-blue-500"
          />
        </div>
        <div>
          <Label>Tip sobe *</Label>
          <Select
            value={formData.tipSobe?.toString()}
            onValueChange={(val) => setFormData({ ...formData, tipSobe: parseInt(val) })}
          >
            <SelectTrigger className="border-blue-200">
              <SelectValue />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value={TipSobe.Standard.toString()}>Standard</SelectItem>
              <SelectItem value={TipSobe.Superior.toString()}>Superior</SelectItem>
              <SelectItem value={TipSobe.Deluxe.toString()}>Deluxe</SelectItem>
              <SelectItem value={TipSobe.Apartman.toString()}>Apartman</SelectItem>
              <SelectItem value={TipSobe.Studios.toString()}>Studio</SelectItem>
            </SelectContent>
          </Select>
        </div>
      </div>

      <div className="grid grid-cols-2 gap-4">
        <div>
          <Label>Cijena po noći (KM) *</Label>
          <Input
            type="number"
            value={formData.cijenaPoNociBAM}
            onChange={(e) => setFormData({ ...formData, cijenaPoNociBAM: parseFloat(e.target.value) })}
            className="border-blue-200"
          />
        </div>
        <div>
          <Label>Maksimalno gostiju *</Label>
          <Input
            type="number"
            value={formData.maksimalnoGostiju}
            onChange={(e) => setFormData({ ...formData, maksimalnoGostiju: parseInt(e.target.value) })}
            className="border-blue-200"
          />
        </div>
      </div>

      <div className="grid grid-cols-2 gap-4">
        <div>
          <Label>Broj kreveta *</Label>
          <Input
            type="number"
            value={formData.brojKreveta}
            onChange={(e) => setFormData({ ...formData, brojKreveta: parseInt(e.target.value) })}
            className="border-blue-200"
          />
        </div>
        <div>
          <Label>Broj bračnih kreveta</Label>
          <Input
            type="number"
            value={formData.brojBracnihKreveta || 0}
            onChange={(e) => setFormData({ ...formData, brojBracnihKreveta: parseInt(e.target.value) })}
            className="border-blue-200"
          />
        </div>
      </div>

      <div>
        <Label>Opis</Label>
        <Textarea
          value={formData.opis || ''}
          onChange={(e) => setFormData({ ...formData, opis: e.target.value })}
          placeholder="Unesite opis sobe..."
          className="border-blue-200"
          rows={3}
        />
      </div>

      <div className="space-y-2">
        <Label className="text-blue-800">Oprema</Label>
        <div className="grid grid-cols-2 gap-4">
          <div className="flex items-center justify-between">
            <Label>WiFi</Label>
            <Switch
              checked={formData.imaWiFi}
              onCheckedChange={(checked) => setFormData({ ...formData, imaWiFi: checked })}
            />
          </div>
          <div className="flex items-center justify-between">
            <Label>TV</Label>
            <Switch
              checked={formData.imaTv}
              onCheckedChange={(checked) => setFormData({ ...formData, imaTv: checked })}
            />
          </div>
          <div className="flex items-center justify-between">
            <Label>Klima</Label>
            <Switch
              checked={formData.imaKlimu}
              onCheckedChange={(checked) => setFormData({ ...formData, imaKlimu: checked })}
            />
          </div>
          <div className="flex items-center justify-between">
            <Label>Mini Bar</Label>
            <Switch
              checked={formData.imaMiniBar}
              onCheckedChange={(checked) => setFormData({ ...formData, imaMiniBar: checked })}
            />
          </div>
          <div className="flex items-center justify-between">
            <Label>Kupatilo</Label>
            <Switch
              checked={formData.imaKupatilo}
              onCheckedChange={(checked) => setFormData({ ...formData, imaKupatilo: checked })}
            />
          </div>
          <div className="flex items-center justify-between">
            <Label>Pogled na grad</Label>
            <Switch
              checked={formData.imaPogledNaGrad}
              onCheckedChange={(checked) => setFormData({ ...formData, imaPogledNaGrad: checked })}
            />
          </div>
        </div>
      </div>

      <div className="flex justify-end gap-4 pt-4">
        <Button type="button" variant="outline" onClick={onSuccess}>
          Odustani
        </Button>
        <Button type="submit" className="bg-blue-600 hover:bg-blue-700" disabled={kreirajSobu.isPending}>
          {kreirajSobu.isPending ? 'Kreiranje...' : 'Kreiraj Sobu'}
        </Button>
      </div>
    </form>
  );
}
