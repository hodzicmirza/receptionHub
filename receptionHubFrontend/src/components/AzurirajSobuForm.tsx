// 📁 components/AzurirajSobuForm.tsx
import { useState } from 'react';
import { useAzurirajSobu } from '@/hooks/useSobe';
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
import { TipSobe, StatusSobe, type SobaDto, type AzurirajSobuDto } from '@/types/dtos/soba.dto';

interface AzurirajSobuFormProps {
  soba: SobaDto;
  onSuccess?: () => void;
}

export function AzurirajSobuForm({ soba, onSuccess }: AzurirajSobuFormProps) {
  const [formData, setFormData] = useState<Partial<AzurirajSobuDto>>({
    brojSobe: soba.brojSobe,
    tipSobe: soba.tipSobe,
    maksimalnoGostiju: soba.maksimalnoGostiju,
    brojKreveta: soba.brojKreveta,
    cijenaPoNociBAM: soba.cijenaPoNociBAM,
    opis: soba.opis,
    imaWiFi: soba.imaWiFi,
    imaTv: soba.imaTv,
    imaKlimu: soba.imaKlimu,
    imaMiniBar: soba.imaMiniBar,
    imaKupatilo: soba.imaKupatilo,
    status: soba.status,
  });

  const azurirajSobu = useAzurirajSobu();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      await azurirajSobu.mutateAsync({ id: soba.idSobe, azuriranaSoba: formData });
      toast.success(`Soba ${formData.brojSobe} je uspješno ažurirana`);
      onSuccess?.();
    } catch (error) {
      toast.error('Greška pri ažuriranju sobe');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      {/* Isti inputi kao u KreirajSobuForm, ali sa defaultValue */}
      <div className="grid grid-cols-2 gap-4">
        <div>
          <Label>Broj sobe *</Label>
          <Input
            defaultValue={formData.brojSobe}
            onChange={(e) => setFormData({ ...formData, brojSobe: e.target.value })}
            className="border-blue-200"
          />
        </div>
        <div>
          <Label>Status sobe</Label>
          <Select
            defaultValue={formData.status?.toString()}
            onValueChange={(val) => setFormData({ ...formData, status: parseInt(val) })}
          >
            <SelectTrigger className="border-blue-200">
              <SelectValue />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value={StatusSobe.Slobodna.toString()}>Slobodna</SelectItem>
              <SelectItem value={StatusSobe.Zauzeta.toString()}>Zauzeta</SelectItem>
              <SelectItem value={StatusSobe.Ciscenje.toString()}>Čišćenje</SelectItem>
              <SelectItem value={StatusSobe.Rezervisana.toString()}>Rezervisana</SelectItem>
              <SelectItem value={StatusSobe.VanFunkcije.toString()}>Van funkcije</SelectItem>
            </SelectContent>
          </Select>
        </div>
      </div>

      <div className="grid grid-cols-2 gap-4">
        <div>
          <Label>Cijena po noći (KM)</Label>
          <Input
            type="number"
            defaultValue={formData.cijenaPoNociBAM}
            onChange={(e) => setFormData({ ...formData, cijenaPoNociBAM: parseFloat(e.target.value) })}
            className="border-blue-200"
          />
        </div>
        <div>
          <Label>Tip sobe</Label>
          <Select
            defaultValue={formData.tipSobe?.toString()}
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

      <div>
        <Label>Opis</Label>
        <Textarea
          defaultValue={formData.opis || ''}
          onChange={(e) => setFormData({ ...formData, opis: e.target.value })}
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
              defaultChecked={formData.imaWiFi}
              onCheckedChange={(checked) => setFormData({ ...formData, imaWiFi: checked })}
            />
          </div>
          <div className="flex items-center justify-between">
            <Label>TV</Label>
            <Switch
              defaultChecked={formData.imaTv}
              onCheckedChange={(checked) => setFormData({ ...formData, imaTv: checked })}
            />
          </div>
          <div className="flex items-center justify-between">
            <Label>Klima</Label>
            <Switch
              defaultChecked={formData.imaKlimu}
              onCheckedChange={(checked) => setFormData({ ...formData, imaKlimu: checked })}
            />
          </div>
          <div className="flex items-center justify-between">
            <Label>Mini Bar</Label>
            <Switch
              defaultChecked={formData.imaMiniBar}
              onCheckedChange={(checked) => setFormData({ ...formData, imaMiniBar: checked })}
            />
          </div>
        </div>
      </div>

      <div className="flex justify-end gap-4 pt-4">
        <Button type="button" variant="outline" onClick={onSuccess}>
          Odustani
        </Button>
        <Button type="submit" className="bg-blue-600 hover:bg-blue-700" disabled={azurirajSobu.isPending}>
          {azurirajSobu.isPending ? 'Ažuriranje...' : 'Ažuriraj Sobu'}
        </Button>
      </div>
    </form>
  );
}
