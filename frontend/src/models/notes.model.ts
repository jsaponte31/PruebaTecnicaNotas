export interface Note {
  id: number;
  title: string;
  content: string;
  createdBy: string;
  createdAt: Date;
}

export interface NoteInput {
  title: string;
  content: string;
}