alter table extension_store.downloads
    rename column e_ref to d_e_ref;

alter index extension_store.downloads_e_ref_index rename to downloads_d_e_ref_index;

alter table extension_store.files
    rename column e_ref to f_e_ref;

alter index extension_store.files_e_ref_index rename to files_f_e_ref_index;

