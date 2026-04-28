import api from "./api";
import type {
  SobaDto,
  KreirajSobuDto,
  AzurirajSobuDto,
  StatusSobeDto,
  TipSobe,
  StatusSobe
} from "../types/dtos/soba.dto.ts"

//DEFINISEM SVE API RUTE ZA POZIV
export const getSveSobe = async () => {
  const response = await api.get<SobaDto[]>("/soba")
  return response.data;
}

// mozda moze i id:int - nisam provjerio
export const getSobuPoId = async (id: number) => {
  const response = await api.get<SobaDto>(`/soba/${id}`)
  return response.data
}

export const getSlobodneSobe = async () => {
  const response = await api.get<SobaDto[]>(`/soba/slobodne`)
  return response.data
}

export const getSobuPoBroju = async (brojSobe: number) => {
  const response = await api.get<SobaDto>(`/soba/broj/${brojSobe}`)
  return response.data
}

export const getSobePoStatusu = async (status: StatusSobe) => {
  const response = await api.get<SobaDto[]>(`/soba/status/${status}`)
  return response.data
}

export const getSobuPoTipu = async (tipSobe: TipSobe) => {
  const response = await api.get<SobaDto[]>(`/soba/tip/${tipSobe}`)
  return response.data
}

export const createSobu = async (novaSoba: KreirajSobuDto) => {
  const response = await api.post<SobaDto>("/soba", novaSoba)
  return response.data;
}

export const updateSobu = async (id: number, updateSoba: AzurirajSobuDto) => {
  const response = await api.put<SobaDto>(`/soba/${id}`, updateSoba)
  return response.data;
}

export const promjeniStatusSobe = async (id: number, statusSobe: StatusSobeDto) => {
  const response = await api.patch<SobaDto>(`/soba/${id}/status`, statusSobe)
  return response.data
}

export const deleteSobu = async (id: number) => {
  const response = await api.delete(`/soba/${id}`)
  return response.data;
}

