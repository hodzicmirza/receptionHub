// 📁 components/SobaDetaljiDialog.tsx
import { SobaDto, TipSobe, StatusSobe } from '@/types/dtos/soba.dto';
import { Badge } from '@/components/ui/badge';
import { Wifi, Tv, Wind, Coffee, Bath, BedDouble, Eye, Microwave, Volume2, Maximize } from 'lucide-react';

interface SobaDetaljiDialogProps {
  soba: SobaDto;
}

export function SobaDetaljiDialog({ soba }: SobaDetaljiDialogProps) {
  const getStatusText = (status: StatusSobe) => {
    switch (status) {
      case StatusSobe.Slobodna: return 'Slobodna';
      case StatusSobe.Zauzeta: return 'Zauzeta';
      case StatusSobe.Ciscenje: return 'Čišćenje';
      case StatusSobe.Rezervisana: return 'Rezervisana';
      case StatusSobe.VanFunkcije: return 'Van funkcije';
    }
  };

  const getTipText = (tip: TipSobe) => {
    switch (tip) {
      case TipSobe.Standard: return 'Standard';
      case TipSobe.Superior: return 'Superior';
      case TipSobe.Deluxe: return 'Deluxe';
      case TipSobe.Apartman: return 'Apartman';
      case TipSobe.Studios: return 'Studio';
    }
  };

  return (
    <div className="space-y-6">
      {/* Slika - placeholder */}
      <div className="bg-blue-100 rounded-lg h-48 flex items-center justify-center">
        <span className="text-blue-400">Slika sobe</span>
      </div>

      {/* Osnovne informacije */}
      <div className="grid grid-cols-2 gap-4">
        <div>
          <p className="text-sm text-muted-foreground">Broj sobe</p>
          <p className="text-lg font-semibold">{soba.brojSobe}</p>
        </div>
        <div>
          <p className="text-sm text-muted-foreground">Tip sobe</p>
          <p className="text-lg font-semibold">{getTipText(soba.tipSobe)}</p>
        </div>
        <div>
          <p className="text-sm text-muted-foreground">Cijena po noći</p>
          <p className="text-lg font-semibold text-green-600">{soba.cijenaPoNociBAM} KM</p>
        </div>
        <div>
          <p className="text-sm text-muted-foreground">Status</p>
          <div>{getStatusText(soba.status)}</div>
        </div>
        <div>
          <p className="text-sm text-muted-foreground">Maksimalno gostiju</p>
          <p>{soba.maksimalnoGostiju} osobe</p>
        </div>
        <div>
          <p className="text-sm text-muted-foreground">Broj kreveta</p>
          <p>{soba.brojKreveta} komada</p>
        </div>
      </div>

      {/* Oprema */}
      <div>
        <p className="text-sm font-semibold mb-2">Oprema u sobi</p>
        <div className="grid grid-cols-2 gap-2">
          {soba.imaWiFi && <div className="flex items-center gap-2"><Wifi className="h-4 w-4" /> WiFi</div>}
          {soba.imaTv && <div className="flex items-center gap-2"><Tv className="h-4 w-4" /> TV</div>}
          {soba.imaKlimu && <div className="flex items-center gap-2"><Wind className="h-4 w-4" /> Klima</div>}
          {soba.imaMiniBar && <div className="flex items-center gap-2"><Coffee className="h-4 w-4" /> Mini Bar</div>}
          {soba.imaKupatilo && <div className="flex items-center gap-2"><Bath className="h-4 w-4" /> Kupatilo</div>}
          {soba.imaTus && <div className="flex items-center gap-2"><Volume2 className="h-4 w-4" /> Tuš</div>}
          {soba.imaRadniSto && <div className="flex items-center gap-2"><Maximize className="h-4 w-4" /> Radni sto</div>}
          {soba.imaFen && <div className="flex items-center gap-2"><Microwave className="h-4 w-4" /> Fen</div>}
        </div>
      </div>

      {/* Opis */}
      {soba.opis && (
        <div>
          <p className="text-sm font-semibold mb-1">Opis</p>
          <p className="text-sm text-muted-foreground">{soba.opis}</p>
        </div>
      )}

      {/* Napomena */}
      {soba.napomena && (
        <div>
          <p className="text-sm font-semibold mb-1">Napomena</p>
          <p className="text-sm text-muted-foreground">{soba.napomena}</p>
        </div>
      )}
    </div>
  );
}
