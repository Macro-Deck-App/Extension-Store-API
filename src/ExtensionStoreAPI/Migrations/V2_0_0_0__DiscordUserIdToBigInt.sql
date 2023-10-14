alter table extension_store.extensions
    alter column e_discord_author_userid type bigint using e_discord_author_userid::bigint;