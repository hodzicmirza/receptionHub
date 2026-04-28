import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import {
  getSveSobe,
  getSobuPoId,
  getSlobodneSobe,
  getSobePoStatusu,
  getSobuPoTipu,
  getSobuPoBroju,
  promjeniStatusSobe,
  createSobu,
  updateSobu,
  deleteSobu
} from "../services/roomService.ts"

import {
  KreirajSobuDto,
  AzurirajSobuDto,
  StatusSobeDto,
  StatusSobe,
  TipSobe
} from "../types/dtos/soba.dto.ts"

// SVE SOBE
export const useSveSobe = () => {
  return useQuery({
    queryKey: ['sobe'],
    queryFn: getSveSobe
  })
}

// SOBA PO ID
export const useSobuPoId = (id: number) => {
  return useQuery({
    queryKey: ['sobe', id],
    queryFn: () => getSobuPoId(id),
    enabled: !!id && id > 0
  })
}

// SLOBODNE SOBE
export const useSlobodneSobe = () => {
  return useQuery({
    queryKey: ['sobe', 'slobodne'],
    queryFn: getSlobodneSobe,
  })
}

// SOBE PO STATUSU
export const useSobePoStatusu = (statusSobe: StatusSobe) => {
  return useQuery({
    queryKey: ['sobe', 'status', statusSobe],
    queryFn: () => getSobePoStatusu(statusSobe!),
    enabled: statusSobe !== null
  })
}

// SOBE PO TIPU
export const useSobePoTipu = (tipSobe: TipSobe) => {
  return useQuery({
    queryKey: ['sobe', 'tip', tipSobe],
    queryFn: () => getSobuPoTipu(tipSobe!),
    enabled: tipSobe !== null
  })
}

// SOBE PO BROJU
export const useSobePoBroju = (brojSobe: number) => {
  return useQuery({
    queryKey: ['sobe', 'broj', brojSobe],
    queryFn: () => getSobuPoBroju(brojSobe),
    enabled: brojSobe > 0 && brojSobe !== undefined
  })
}

// KREIRAJ SOBU
export const useKreirajSobu = () => {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (novaSoba: KreirajSobuDto) => createSobu(novaSoba),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ['sobe']
      })
    }
  })
}

// AZURIRAJ SOBU
export const useAzurirajSobu = () => {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: ({ id, azuriranaSoba }: { id: number, azuriranaSoba: AzurirajSobuDto }) => updateSobu(id!, azuriranaSoba!),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['sobe']
      })
      queryClient.invalidateQueries({
        queryKey: ['soba', variables.id]
      })
    }
  })
}

// PROMJENI STATUS SOBE
export const usePromjeniStatusSobe = () => {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: ({ id, noviStatus }: { id: number, noviStatus: StatusSobeDto }) => promjeniStatusSobe(id, noviStatus),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['soba', variables.id] })
      queryClient.invalidateQueries({ queryKey: ['sobe'] })
    }
  })
}

// OBRISI SOBU
export const useObrisiSobu = () => {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (id: number) => deleteSobu(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['sobe'] })
    }
  })
}
