export interface GostStatistikaDto {
  ukupno: number;
  danas: number;
  ovajMjesec: number;
  fizickaLica: number;
  pravnaLica: number;
  vip: number;
  topDrzave: DrzavaStatistika[];
}

export interface DrzavaStatistika {
  drzava: string;
  broj: number;
}