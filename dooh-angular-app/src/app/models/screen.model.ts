export interface Screen {
  id?: number;
  name: string;
  location: string;
  status: ScreenStatus;
  resolution?: string;
  orientation?: string;
}

export enum ScreenStatus {
  Active = 'Active',
  Inactive = 'Inactive',
  Maintenance = 'Maintenance'
}

export interface ScreenStatusUpdate {
  status: ScreenStatus;
}
