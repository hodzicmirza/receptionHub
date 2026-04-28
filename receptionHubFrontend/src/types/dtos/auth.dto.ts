export interface LoginDto {
  korisnickoIme: string;
  lozinka: string;
}

export interface TokenDto {
  token: string;
  istice: string; // ISO datetime string
  tip: string;    // "Bearer"
  ime: string;
  prezime: string;
  korisnickoIme: string;
  pozicija: string; // TipPozicije
}

export type LoginResponseDto = TokenDto;