export interface Ad {
  id?: number;
  title: string;            
  durationSeconds: number;  
  mediaUrl?: string;
  mediaType?: string;
  fileSize?: number;
  createdAt?: string;
}

export interface AdUpload {
  title: string;            
  durationSeconds: number; 
  file: File;
}