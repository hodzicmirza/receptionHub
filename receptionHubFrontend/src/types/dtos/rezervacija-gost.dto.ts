export interface RezervacijaGostDto {
  id: number;
  rezervacijaId: number;
  gostId: number;
  imeGosta?: string;
  prezimeGosta?: string;
  nazivFirme?: string;
  jeGlavniGost: boolean;
  posebneNapomene?: string;
  brojRezervacije?: string;
}

export interface KreirajRezervacijaGostDto {
  rezervacijaId: number;
  gostId: number;
  jeGlavniGost?: boolean;
  posebneNapomene?: string;
}

export interface AzurirajRezervacijaGostDto {
  jeGlavniGost?: boolean;
  posebneNapomene?: string;
}