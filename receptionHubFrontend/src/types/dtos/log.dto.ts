export enum TipLoga {
  Info = 0,
  Upozorenje = 1,
  Greska = 2,
  Debug = 3,
  Audit = 4,
  Http = 5,
  Izuzetak = 6
}

export interface LogDto {
  id: number;
  vrijeme: string;
  tip: TipLoga;
  poruka: string;
  detalji?: string;
  izvor?: string;
  recepcionerId?: number;
  gostId?: number;
  korisnickoIme?: string;
  httpMetoda?: string;
  putanja?: string;
  statusKod?: number;
  ipAdresa?: string;
  userAgent?: string;
  zaglavlja?: string;
  tijeloZahtjeva?: string;
  trajanjeMs?: number;
  tipIzuzetka?: string;
  stackTrace?: string;
  traceId?: string;
  sessionId?: string;
  dodatniPodaci?: string;
}