export enum TipSobe {
  Standard = 0,
  Superior = 1,
  Deluxe = 2,
  Apartman = 3,
  Studios = 4
}

export enum StatusSobe {
  Slobodna = 0,
  Zauzeta = 1,
  Ciscenje = 2,
  Rezervisana = 3,
  VanFunkcije = 4
}

export interface SobaDto {
  idSobe: number;
  brojSobe: string;
  tipSobe: TipSobe;
  maksimalnoGostiju: number;
  brojKreveta: number;
  brojBracnihKreveta?: number;
  brojOdvojenihKreveta?: number;
  imaDodatniKrevet: boolean;
  cijenaPoNociBAM: number;
  opis?: string;
  kratkiOpis?: string;
  
  // Oprema
  imaTv: boolean;
  imaKlimu: boolean;
  imaMiniBar: boolean;
  imaPogledNaGrad: boolean;
  imaWiFi: boolean;
  imaRadniSto: boolean;
  imaFen: boolean;
  imaPeglu: boolean;
  imaKupatilo: boolean;
  imaTus: boolean;
  
  // Status
  status: StatusSobe;
  planiranoOslobadjanje?: string; // ISO date string
  
  // Slike i napomene
  glavnaSlika?: string;
  napomena?: string;
  
  // Metadata
  vrijemeKreiranja: string;
  kreiraoRecepcionerId?: number;
  vrijemeAzuriranja?: string;
  
  // Dodatno
  brojAktivnihRezervacija: number;
}

export interface KreirajSobuDto {
  brojSobe: string;
  tipSobe: TipSobe;
  maksimalnoGostiju: number;
  brojKreveta: number;
  brojBracnihKreveta?: number;
  brojOdvojenihKreveta?: number;
  imaDodatniKrevet?: boolean;
  cijenaPoNociBAM: number;
  opis?: string;
  kratkiOpis?: string;
  
  imaTv?: boolean;
  imaKlimu?: boolean;
  imaMiniBar?: boolean;
  imaPogledNaGrad?: boolean;
  imaWiFi?: boolean;
  imaRadniSto?: boolean;
  imaFen?: boolean;
  imaPeglu?: boolean;
  imaKupatilo?: boolean;
  imaTus?: boolean;
  
  glavnaSlika?: string;
  napomena?: string;
}

export interface AzurirajSobuDto {
  brojSobe?: string;
  tipSobe?: TipSobe;
  maksimalnoGostiju?: number;
  brojKreveta?: number;
  brojBracnihKreveta?: number;
  brojOdvojenihKreveta?: number;
  imaDodatniKrevet?: boolean;
  cijenaPoNociBAM?: number;
  opis?: string;
  kratkiOpis?: string;
  
  imaTv?: boolean;
  imaKlimu?: boolean;
  imaMiniBar?: boolean;
  imaPogledNaGrad?: boolean;
  imaWiFi?: boolean;
  imaRadniSto?: boolean;
  imaFen?: boolean;
  imaPeglu?: boolean;
  imaKupatilo?: boolean;
  imaTus?: boolean;
  
  status?: StatusSobe;
  planiranoOslobadjanje?: string;
  
  glavnaSlika?: string;
  napomena?: string;
}

export interface StatusSobeDto {
  status: StatusSobe;
  planiranoOslobadjanje?: string;
}