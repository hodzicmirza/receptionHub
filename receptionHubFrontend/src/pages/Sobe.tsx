// 📁 app/sobe/page.tsx
import { useState } from 'react';
import { useSveSobe, useObrisiSobu, usePromjeniStatusSobe } from '@/hooks/useSobe';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Input } from '@/components/ui/input';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from '@/components/ui/table';
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle
} from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { toast } from 'sonner';
import {
  Plus,
  Pencil,
  Trash2,
  RefreshCw,
  Eye,
  TrendingUp,
  Users,
  Home,
  Activity,
  Search,
  ChevronDown,
  MoreHorizontal,
  Filter
} from 'lucide-react';
import { StatusSobe, TipSobe } from '@/types/dtos/soba.dto';
import { KreirajSobuForm } from '../components/KreirajSobuForm';
import { AzurirajSobuForm } from '../components/AzurirajSobuForm';
import { SobaDetaljiDialog } from '../components/SobaDetaljiDialog';

export default function SobePage() {
  const { data: sobe, isLoading, isError, error, refetch } = useSveSobe();
  const obrisiSobu = useObrisiSobu();
  const promijeniStatus = usePromjeniStatusSobe();

  const [selectedSoba, setSelectedSoba] = useState<any>(null);
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false);
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
  const [isDetailsDialogOpen, setIsDetailsDialogOpen] = useState(false);
  const [filterStatus, setFilterStatus] = useState<string>("all");
  const [filterTip, setFilterTip] = useState<string>("all");
  const [searchQuery, setSearchQuery] = useState<string>("");

  // Filtriranje soba
  const filteredSobe = sobe?.filter(soba => {
    if (filterStatus !== "all" && soba.status !== parseInt(filterStatus)) return false;
    if (filterTip !== "all" && soba.tipSobe !== parseInt(filterTip)) return false;
    if (searchQuery && !soba.brojSobe.toLowerCase().includes(searchQuery.toLowerCase())) return false;
    return true;
  });

  // Statistika
  const stats = {
    ukupno: sobe?.length || 0,
    slobodne: sobe?.filter(s => s.status === StatusSobe.Slobodna).length || 0,
    zauzete: sobe?.filter(s => s.status === StatusSobe.Zauzeta).length || 0,
    rezervisane: sobe?.filter(s => s.status === StatusSobe.Rezervisana).length || 0,
    ciscenje: sobe?.filter(s => s.status === StatusSobe.Ciscenje).length || 0,
    prihod: sobe?.reduce((sum, s) => sum + (s.status === StatusSobe.Zauzeta ? s.cijenaPoNociBAM : 0), 0) || 0,
  };

  const handleDelete = async (id: number, brojSobe: string) => {
    if (confirm(`Da li ste sigurni da želite obrisati sobu ${brojSobe}?`)) {
      try {
        await obrisiSobu.mutateAsync(id);
        toast.success(`Soba ${brojSobe} je uspješno obrisana`);
      } catch (error) {
        toast.error('Greška pri brisanju sobe');
      }
    }
  };

  const handleStatusChange = async (id: number, noviStatus: number, brojSobe: string) => {
    try {
      await promijeniStatus.mutateAsync({ id, noviStatus: { status: noviStatus } });
      toast.success(`Status sobe ${brojSobe} je promijenjen`);
    } catch (error) {
      toast.error('Greška pri promjeni statusa');
    }
  };

  const getStatusBadge = (status: StatusSobe) => {
    switch (status) {
      case StatusSobe.Slobodna:
        return <Badge className="bg-green-500 hover:bg-green-600">Slobodna</Badge>;
      case StatusSobe.Zauzeta:
        return <Badge className="bg-red-500 hover:bg-red-600">Zauzeta</Badge>;
      case StatusSobe.Ciscenje:
        return <Badge className="bg-yellow-500 hover:bg-yellow-600">Čišćenje</Badge>;
      case StatusSobe.Rezervisana:
        return <Badge className="bg-blue-500 hover:bg-blue-600">Rezervisana</Badge>;
      case StatusSobe.VanFunkcije:
        return <Badge className="bg-gray-500 hover:bg-gray-600">Van funkcije</Badge>;
      default:
        return <Badge variant="outline">Nepoznat</Badge>;
    }
  };

  const getStatusColor = (status: StatusSobe) => {
    switch (status) {
      case StatusSobe.Slobodna: return 'text-green-600';
      case StatusSobe.Zauzeta: return 'text-red-600';
      case StatusSobe.Ciscenje: return 'text-yellow-600';
      case StatusSobe.Rezervisana: return 'text-blue-600';
      default: return 'text-gray-600';
    }
  };

  const getTipSobeText = (tip: TipSobe) => {
    switch (tip) {
      case TipSobe.Standard: return "Standard";
      case TipSobe.Superior: return "Superior";
      case TipSobe.Deluxe: return "Deluxe";
      case TipSobe.Apartman: return "Apartman";
      case TipSobe.Studios: return "Studio";
      default: return "Nepoznat";
    }
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-96">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="flex flex-col items-center justify-center h-96 gap-4">
        <p className="text-red-500">Greška: {error?.message}</p>
        <Button onClick={() => refetch()} variant="outline">
          <RefreshCw className="mr-2 h-4 w-4" />
          Pokušaj ponovo
        </Button>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header Navigation - kao na slici */}
      <div className="border-b bg-white">
        <div className="flex h-16 items-center px-6">
          <div className="flex items-center gap-2 font-semibold text-xl text-blue-800">
            <Home className="h-5 w-5" />
            <span>ReceptionHub</span>
          </div>
          <div className="ml-8 flex items-center space-x-6 text-sm">
            <span className="text-blue-600 font-medium border-b-2 border-blue-600 pb-4">Sobe</span>
            <span className="text-gray-500">Rezervacije</span>
            <span className="text-gray-500">Gosti</span>
            <span className="text-gray-500">Recepcioneri</span>
            <span className="text-gray-500">Logovi</span>
          </div>
          <div className="ml-auto flex items-center gap-4">
            <div className="relative">
              <Search className="absolute left-2 top-2.5 h-4 w-4 text-gray-400" />
              <Input
                placeholder="Pretraži sobe..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className="pl-8 w-64"
              />
            </div>
            <div className="flex items-center gap-2">
              <div className="h-8 w-8 rounded-full bg-blue-600 flex items-center justify-center text-white text-sm font-medium">
                MH
              </div>
              <ChevronDown className="h-4 w-4 text-gray-500" />
            </div>
          </div>
        </div>
      </div>

      <div className="p-6 space-y-6">
        {/* Welcome Section */}
        <div className="flex justify-between items-center">
          <div>
            <h1 className="text-2xl font-semibold text-gray-900">Upravljanje Sobama</h1>
            <p className="text-sm text-gray-500 mt-1">Pregled i upravljanje svim sobama u hotelu</p>
          </div>
          <Dialog open={isCreateDialogOpen} onOpenChange={setIsCreateDialogOpen}>
            <DialogTrigger asChild>
              <Button className="bg-blue-600 hover:bg-blue-700">
                <Plus className="mr-2 h-4 w-4" />
                Dodaj Novu Sobu
              </Button>
            </DialogTrigger>
            <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
              <DialogHeader>
                <DialogTitle className="text-2xl font-bold text-blue-800">
                  Kreiraj Novu Sobu
                </DialogTitle>
              </DialogHeader>
              <KreirajSobuForm onSuccess={() => setIsCreateDialogOpen(false)} />
            </DialogContent>
          </Dialog>
        </div>

        {/* Statistics Cards - kao na slici */}
        <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
          <Card className="border-l-4 border-l-green-500 shadow-sm">
            <CardContent className="p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-500">Slobodne Sobe</p>
                  <p className="text-2xl font-bold text-gray-900">{stats.slobodne}</p>
                  <p className="text-xs text-green-600 mt-1">↑ Dostupno za rezervaciju</p>
                </div>
                <div className="h-12 w-12 rounded-full bg-green-100 flex items-center justify-center">
                  <Home className="h-6 w-6 text-green-600" />
                </div>
              </div>
            </CardContent>
          </Card>

          <Card className="border-l-4 border-l-red-500 shadow-sm">
            <CardContent className="p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-500">Zauzete Sobe</p>
                  <p className="text-2xl font-bold text-gray-900">{stats.zauzete}</p>
                  <p className="text-xs text-red-600 mt-1">↑ 20% ovaj mjesec</p>
                </div>
                <div className="h-12 w-12 rounded-full bg-red-100 flex items-center justify-center">
                  <Users className="h-6 w-6 text-red-600" />
                </div>
              </div>
            </CardContent>
          </Card>

          <Card className="border-l-4 border-l-blue-500 shadow-sm">
            <CardContent className="p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-500">Rezervisane Sobe</p>
                  <p className="text-2xl font-bold text-gray-900">{stats.rezervisane}</p>
                  <p className="text-xs text-blue-600 mt-1">Buduće rezervacije</p>
                </div>
                <div className="h-12 w-12 rounded-full bg-blue-100 flex items-center justify-center">
                  <Activity className="h-6 w-6 text-blue-600" />
                </div>
              </div>
            </CardContent>
          </Card>

          <Card className="border-l-4 border-l-purple-500 shadow-sm">
            <CardContent className="p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-500">Dnevni Prihod</p>
                  <p className="text-2xl font-bold text-gray-900">${stats.prihod}</p>
                  <p className="text-xs text-purple-600 mt-1">Trending up this month</p>
                </div>
                <div className="h-12 w-12 rounded-full bg-purple-100 flex items-center justify-center">
                  <TrendingUp className="h-6 w-6 text-purple-600" />
                </div>
              </div>
            </CardContent>
          </Card>
        </div>

        {/* Filteri i Tabela */}
        <Card className="shadow-sm">
          <CardHeader className="border-b bg-gray-50/50">
            <div className="flex justify-between items-center">
              <CardTitle className="text-lg font-semibold">Lista Soba</CardTitle>
              <div className="flex items-center gap-2">
                <Button variant="outline" size="sm" onClick={() => refetch()}>
                  <RefreshCw className="h-4 w-4 mr-1" />
                  Osvježi
                </Button>
                <Button variant="outline" size="sm">
                  <Filter className="h-4 w-4 mr-1" />
                  Filteri
                </Button>
                <Button variant="outline" size="sm">
                  <MoreHorizontal className="h-4 w-4" />
                </Button>
              </div>
            </div>
          </CardHeader>
          <CardContent className="p-0">
            {/* Filteri */}
            <div className="p-4 border-b bg-white">
              <div className="flex flex-wrap gap-4">
                <div className="flex items-center gap-2">
                  <Label className="text-sm text-gray-500">Status:</Label>
                  <Select value={filterStatus} onValueChange={setFilterStatus}>
                    <SelectTrigger className="w-[140px] h-9">
                      <SelectValue placeholder="Svi statusi" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="all">Svi statusi</SelectItem>
                      <SelectItem value={StatusSobe.Slobodna.toString()}>Slobodna</SelectItem>
                      <SelectItem value={StatusSobe.Zauzeta.toString()}>Zauzeta</SelectItem>
                      <SelectItem value={StatusSobe.Ciscenje.toString()}>Čišćenje</SelectItem>
                      <SelectItem value={StatusSobe.Rezervisana.toString()}>Rezervisana</SelectItem>
                      <SelectItem value={StatusSobe.VanFunkcije.toString()}>Van funkcije</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
                <div className="flex items-center gap-2">
                  <Label className="text-sm text-gray-500">Tip sobe:</Label>
                  <Select value={filterTip} onValueChange={setFilterTip}>
                    <SelectTrigger className="w-[140px] h-9">
                      <SelectValue placeholder="Svi tipovi" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="all">Svi tipovi</SelectItem>
                      <SelectItem value={TipSobe.Standard.toString()}>Standard</SelectItem>
                      <SelectItem value={TipSobe.Superior.toString()}>Superior</SelectItem>
                      <SelectItem value={TipSobe.Deluxe.toString()}>Deluxe</SelectItem>
                      <SelectItem value={TipSobe.Apartman.toString()}>Apartman</SelectItem>
                      <SelectItem value={TipSobe.Studios.toString()}>Studio</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>
            </div>

            {/* Tabela */}
            <div className="overflow-x-auto">
              <Table>
                <TableHeader>
                  <TableRow className="bg-gray-50">
                    <TableHead className="font-semibold">Broj Sobe</TableHead>
                    <TableHead className="font-semibold">Tip</TableHead>
                    <TableHead className="font-semibold">Cijena (KM)</TableHead>
                    <TableHead className="font-semibold">Kapacitet</TableHead>
                    <TableHead className="font-semibold">Status</TableHead>
                    <TableHead className="font-semibold text-right">Akcije</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {filteredSobe?.map((soba) => (
                    <TableRow key={soba.idSobe} className="hover:bg-gray-50 transition-colors">
                      <TableCell className="font-medium">{soba.brojSobe}</TableCell>
                      <TableCell>{getTipSobeText(soba.tipSobe)}</TableCell>
                      <TableCell>{soba.cijenaPoNociBAM} KM</TableCell>
                      <TableCell>{soba.maksimalnoGostiju} gostiju</TableCell>
                      <TableCell>
                        <div className="flex items-center gap-2">
                          <span className={`h-2 w-2 rounded-full ${getStatusColor(soba.status)} bg-current`}></span>
                          {getStatusBadge(soba.status)}
                        </div>
                      </TableCell>
                      <TableCell className="text-right">
                        <div className="flex justify-end gap-1">
                          <Button
                            variant="ghost"
                            size="icon"
                            className="h-8 w-8 text-blue-600 hover:text-blue-800 hover:bg-blue-50"
                            onClick={() => {
                              setSelectedSoba(soba);
                              setIsDetailsDialogOpen(true);
                            }}
                          >
                            <Eye className="h-4 w-4" />
                          </Button>
                          <Button
                            variant="ghost"
                            size="icon"
                            className="h-8 w-8 text-green-600 hover:text-green-800 hover:bg-green-50"
                            onClick={() => {
                              setSelectedSoba(soba);
                              setIsEditDialogOpen(true);
                            }}
                          >
                            <Pencil className="h-4 w-4" />
                          </Button>
                          <Select
                            value={soba.status.toString()}
                            onValueChange={(value) => handleStatusChange(soba.idSobe, parseInt(value), soba.brojSobe)}
                          >
                            <SelectTrigger className="h-8 w-[110px] text-xs">
                              <SelectValue />
                            </SelectTrigger>
                            <SelectContent>
                              <SelectItem value={StatusSobe.Slobodna.toString()}>Slobodna</SelectItem>
                              <SelectItem value={StatusSobe.Zauzeta.toString()}>Zauzeta</SelectItem>
                              <SelectItem value={StatusSobe.Ciscenje.toString()}>Čišćenje</SelectItem>
                              <SelectItem value={StatusSobe.Rezervisana.toString()}>Rezervisana</SelectItem>
                              <SelectItem value={StatusSobe.VanFunkcije.toString()}>Van funkcije</SelectItem>
                            </SelectContent>
                          </Select>
                          <Button
                            variant="ghost"
                            size="icon"
                            className="h-8 w-8 text-red-600 hover:text-red-800 hover:bg-red-50"
                            onClick={() => handleDelete(soba.idSobe, soba.brojSobe)}
                          >
                            <Trash2 className="h-4 w-4" />
                          </Button>
                        </div>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </div>
            {filteredSobe?.length === 0 && (
              <div className="text-center py-8 text-gray-500">
                Nema soba za prikaz
              </div>
            )}
          </CardContent>
        </Card>
      </div>

      {/* Footer - kao na slici */}
      <footer className="border-t bg-white mt-6">
        <div className="flex justify-between items-center p-4 text-sm text-gray-500">
          <p>© 2024 ReceptionHub. Sva prava zadržana.</p>
          <div className="flex items-center gap-4">
            <span>Mirza Hodžić</span>
            <button className="text-red-500 hover:text-red-700">Odjavi se</button>
          </div>
        </div>
      </footer>

      {/* Dialogs */}
      <Dialog open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen}>
        <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle className="text-2xl font-bold text-blue-800">
              Ažuriraj Sobu {selectedSoba?.brojSobe}
            </DialogTitle>
          </DialogHeader>
          {selectedSoba && (
            <AzurirajSobuForm
              soba={selectedSoba}
              onSuccess={() => setIsEditDialogOpen(false)}
            />
          )}
        </DialogContent>
      </Dialog>

      <Dialog open={isDetailsDialogOpen} onOpenChange={setIsDetailsDialogOpen}>
        <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle className="text-2xl font-bold text-blue-800">
              Detalji Sobe {selectedSoba?.brojSobe}
            </DialogTitle>
          </DialogHeader>
          {selectedSoba && (
            <SobaDetaljiDialog soba={selectedSoba} />
          )}
        </DialogContent>
      </Dialog>
    </div>
  );
}
