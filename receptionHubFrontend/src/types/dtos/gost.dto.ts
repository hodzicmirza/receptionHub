export enum TipGosta {
  FizickoLice = 0,
  PravnoLice = 1
}

export interface GostDto {
  idGosta: number;
  tipGosta: TipGosta;
  ime?: string;
  prezime?: string;
  nazivFirme?: string;
  kontaktOsoba?: string;
  brojTelefona?: string;
  email?: string;
  drzava?: string;
  tipDokumenta: string;
  slikaDokumenta?: string;
  vipGost: boolean;
  dodatno?: string;
  vrijemeKreiranja: string; // ISO datetime string
  recepcionerId: number;
  brojRezervacija: number;
}

export interface KreirajGostaDto {
  tipGosta: TipGosta;
  ime?: string;
  prezime?: string;
  nazivFirme?: string;
  kontaktOsoba?: string;
  brojTelefona?: string;
  email?: string;
  drzava?: string;
  tipDokumenta: string;
  slikaDokumenta: string;
  vipGost?: boolean;
  dodatno?: string;
}

export interface AzurirajGostaDto {
  ime?: string;
  prezime?: string;
  nazivFirme?: string;
  kontaktOsoba?: string;
  brojTelefona?: string;
  email?: string;
  drzava?: string;
  tipDokumenta?: string;
  slikaDokumenta?: string;
  vipGost?: boolean;
  dodatno?: string;
}