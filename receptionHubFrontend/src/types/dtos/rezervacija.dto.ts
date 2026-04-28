export enum NacinRezervacije {
  Direktno = 0,
  Telefonski = 1,
  Email = 2,
  Booking = 3,
  Expedia = 4,
  Agoda = 5,
  AirBnB = 6
}

export enum StatusRezervacije {
  Potvrdjena = 0,
  NaCekanju = 1,
  Otkazana = 2,
  Zavrsena = 3,
  NijeSePojavio = 4
}

export interface RezervacijaDto {
  idRezervacije: number;
  brojRezervacije?: string;
  sobaId: number;
  brojSobe?: string;
  recepcionerId: number;
  imeRecepcionera?: string;
  datumDolaska: string; // ISO date string
  datumOdlaska: string; // ISO date string
  brojNocenja: number;
  brojOdraslih: number;
  brojDjece: number;
  ukupnoGostiju: number;
  cijenaPoNoci: number;
  popust?: number;
  ukupnaCijena: number;
  nacinRezervacije: NacinRezervacije;
  eksterniBrojRezervacije?: string;
  status: StatusRezervacije;
  zahtjevi?: string;
  napomena?: string;
  vrijemeKreiranja: string; // ISO datetime string
  gosti?: RezervacijaGostDto[];
}

export interface KreirajRezervacijuDto {
  sobaId: number;
  datumDolaska: string; // ISO date string
  datumOdlaska: string; // ISO date string
  brojOdraslih: number;
  brojDjece?: number;
  cijenaPoNoci: number;
  popust?: number;
  nacinRezervacije: NacinRezervacije;
  eksterniBrojRezervacije?: string;
  zahtjevi?: string;
  napomena?: string;
  gostIds?: number[];
  glavniGostId?: number;
}

export interface AzurirajRezervacijuDto {
  sobaId?: number;
  datumDolaska?: string;
  datumOdlaska?: string;
  brojOdraslih?: number;
  brojDjece?: number;
  cijenaPoNoci?: number;
  popust?: number;
  nacinRezervacije?: NacinRezervacije;
  eksterniBrojRezervacije?: string;
  status?: StatusRezervacije;
  zahtjevi?: string;
  napomena?: string;
}

export interface KratkiPregledRezervacijeDto {
  idRezervacije: number;
  brojRezervacije?: string;
  brojSobe?: string;
  datumDolaska: string;
  datumOdlaska: string;
  brojNocenja: number;
  ukupnoGostiju: number;
  ukupnaCijena: number;
  status: StatusRezervacije;
  imeGlavnogGosta?: string;
}

export interface StatusRezervacijeDto {
  status: StatusRezervacije;
  razlog?: string;
}

export interface OtkaziRezervacijuDto {
  razlog: string;
}