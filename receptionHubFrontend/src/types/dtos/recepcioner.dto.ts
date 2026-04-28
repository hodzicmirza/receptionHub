export type TipPozicije = 'Recepcioner' | 'Supervizor' | 'Menadzer' | 'Admin';

export const TipPozicijeValues = {
  Recepcioner: 'Recepcioner' as TipPozicije,
  Supervizor: 'Supervizor' as TipPozicije,
  Menadzer: 'Menadzer' as TipPozicije,
  Admin: 'Admin' as TipPozicije
};

export interface RecepcionerDto {
  idRecepcionera: number;
  ime: string;
  prezime: string;
  korisnickoIme: string;
  email?: string;
  brojTelefona?: string;
  pozicija: TipPozicije;
  aktivan: boolean;
  datumKreiranjaRacuna: string;
  posljednjiLogin?: string;
  slikaProfila?: string;
  napomena?: string;
}

export interface KreirajRecepcioneraDto {
  ime: string;
  prezime: string;
  korisnickoIme: string;
  email?: string;
  brojTelefona?: string;
  pozicija: TipPozicije;
  lozinka: string;
  slikaProfila?: string;
  napomena?: string;
}

export interface UpdateRecepcioneraDto {
  ime?: string;
  prezime?: string;
  email?: string;
  brojTelefona?: string;
  pozicija?: TipPozicije;
  aktivan?: boolean;
  slikaProfila?: string;
  napomena?: string;
}

export interface PromjeniLozinkuDto {
  staraLozinka: string;
  novaLozinka: string;
  potvrdaLozinke: string;
}