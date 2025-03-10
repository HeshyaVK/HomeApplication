PGDMP                  	    |            NewDb    16.3    16.3 ^    v           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            w           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            x           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            y           1262    16398    NewDb    DATABASE     �   CREATE DATABASE "NewDb" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_United Arab Emirates.1252';
    DROP DATABASE "NewDb";
                postgres    false            �            1255    16452    create_test_table() 	   PROCEDURE     �   CREATE PROCEDURE public.create_test_table()
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- Create the test table if it does not exist
    CREATE TABLE IF NOT EXISTS test (
        id SERIAL PRIMARY KEY,
        name VARCHAR(255)
    );
END;
$$;
 +   DROP PROCEDURE public.create_test_table();
       public          postgres    false            �            1255    16461    delete_from_test(integer) 	   PROCEDURE     �   CREATE PROCEDURE public.delete_from_test(IN p_id integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM test WHERE id = p_id;
END;
$$;
 9   DROP PROCEDURE public.delete_from_test(IN p_id integer);
       public          postgres    false            �            1255    16464    get_all_tests()    FUNCTION     �   CREATE FUNCTION public.get_all_tests() RETURNS TABLE(id integer, name character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT * FROM test;
END;
$$;
 &   DROP FUNCTION public.get_all_tests();
       public          postgres    false            �            1255    16465    get_test_by_id(integer)    FUNCTION     �   CREATE FUNCTION public.get_test_by_id(p_id integer) RETURNS TABLE(id integer, name character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT * FROM test WHERE id = p_id;
END;
$$;
 3   DROP FUNCTION public.get_test_by_id(p_id integer);
       public          postgres    false            �            1255    16466    get_test_by_id1(integer)    FUNCTION     �   CREATE FUNCTION public.get_test_by_id1(p_id integer) RETURNS TABLE(id integer, name character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT test.id, test.name FROM test WHERE test.id = p_id;
END;
$$;
 4   DROP FUNCTION public.get_test_by_id1(p_id integer);
       public          postgres    false                       1255    16666    getbookingswithstatus() 	   PROCEDURE       CREATE PROCEDURE public.getbookingswithstatus()
    LANGUAGE plpgsql
    AS $$
BEGIN
    SELECT 
        b."Id", 
        b."User", 
        b."AppointmentDate", 
        b."AppointmentTime", 
        b."Address1", 
        b."Address2", 
        b."City", 
        b."State", 
        b."Pincode", 
        b."Services", 
        b."SendPaymentLink", 
        b."Total", 
        s."Name" AS "StatusName"
    FROM 
        public."Bookings" b
    JOIN 
        public."Statuses" s
    ON 
        b."StatusID" = s."Id";
END;
$$;
 /   DROP PROCEDURE public.getbookingswithstatus();
       public          postgres    false            	           1255    16667    getbookingswithstatus1()    FUNCTION     �  CREATE FUNCTION public.getbookingswithstatus1() RETURNS TABLE("Id" integer, "User" character varying, "AppointmentDate" date, "AppointmentTime" time without time zone, "Address1" character varying, "Address2" character varying, "City" character varying, "State" character varying, "Pincode" character varying, "Services" json, "SendPaymentLink" boolean, "Total" numeric, "StatusName" character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY 
    SELECT 
        b."Id", 
        b."User", 
        b."AppointmentDate", 
        b."AppointmentTime", 
        b."Address1", 
        b."Address2", 
        b."City", 
        b."State", 
        b."Pincode", 
        b."Services", 
        b."SendPaymentLink", 
        b."Total", 
        s."Name" AS "StatusName"
    FROM 
        public."Bookings" b
    JOIN 
        public."Statuses" s
    ON 
        b."StatusID" = s."Id";
END;
$$;
 /   DROP FUNCTION public.getbookingswithstatus1();
       public          postgres    false                       1255    16668    getbookingswithstatuss()    FUNCTION     z  CREATE FUNCTION public.getbookingswithstatuss() RETURNS TABLE("Id" integer, "User" text, "AppointmentDate" date, "AppointmentTime" time without time zone, "Address1" character varying, "Address2" character varying, "City" character varying, "State" character varying, "Pincode" character varying, "Services" json, "SendPaymentLink" boolean, "Total" numeric, "StatusName" character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY 
    SELECT 
        b."Id", 
        b."User", 
        b."AppointmentDate", 
        b."AppointmentTime", 
        b."Address1", 
        b."Address2", 
        b."City", 
        b."State", 
        b."Pincode", 
        b."Services", 
        b."SendPaymentLink", 
        b."Total", 
        s."Name" AS "StatusName"
    FROM 
        public."Bookings" b
    JOIN 
        public."Statuses" s
    ON 
        b."StatusID" = s."Id";
END;
$$;
 /   DROP FUNCTION public.getbookingswithstatuss();
       public          postgres    false            �            1255    16463 #   insert_into_test(character varying) 	   PROCEDURE     �   CREATE PROCEDURE public.insert_into_test(IN p_name character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO test (name) VALUES (p_name);
END;
$$;
 E   DROP PROCEDURE public.insert_into_test(IN p_name character varying);
       public          postgres    false            �            1255    16460    select_from_test() 	   PROCEDURE     �   CREATE PROCEDURE public.select_from_test(OUT result_set refcursor)
    LANGUAGE plpgsql
    AS $$
BEGIN
    OPEN result_set FOR SELECT * FROM test;
END;
$$;
 B   DROP PROCEDURE public.select_from_test(OUT result_set refcursor);
       public          postgres    false                       1255    16613    sync_booking_service()    FUNCTION     �  CREATE FUNCTION public.sync_booking_service() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
    service JSONB;
    service_id INTEGER;
    service_name TEXT;
    price NUMERIC;
    quantity INTEGER;
BEGIN
    -- DELETE operation
    IF TG_OP = 'DELETE' THEN
        DELETE FROM booking_service WHERE booking_id = OLD."Id";
        RETURN OLD;
    END IF;
    
    -- INSERT or UPDATE operation
    DELETE FROM booking_service WHERE booking_id = NEW."Id";

    FOR service IN SELECT * FROM jsonb_array_elements(NEW."Services"::jsonb) LOOP
        service_id := (service->>'id')::INTEGER;
        service_name := service->>'name';
        price := (service->>'price')::NUMERIC;
        quantity := (service->>'quantity')::INTEGER;
        
        INSERT INTO booking_service (booking_id, service_id, service_name, price, quantity)
        VALUES (NEW."Id", service_id, service_name, price, quantity);
    END LOOP;

    RETURN NEW;
END;
$$;
 -   DROP FUNCTION public.sync_booking_service();
       public          postgres    false            �            1255    16462 '   update_test(integer, character varying) 	   PROCEDURE     �   CREATE PROCEDURE public.update_test(IN p_id integer, IN p_name character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE test SET name = p_name WHERE id = p_id;
END;
$$;
 Q   DROP PROCEDURE public.update_test(IN p_id integer, IN p_name character varying);
       public          postgres    false            �            1259    16516    Bookings    TABLE       CREATE TABLE public."Bookings" (
    "Id" integer NOT NULL,
    "User" text NOT NULL,
    "AppointmentDate" text NOT NULL,
    "AppointmentTime" text NOT NULL,
    "Address1" text NOT NULL,
    "Address2" text NOT NULL,
    "City" text NOT NULL,
    "State" text NOT NULL,
    "Pincode" text NOT NULL,
    "Services" text NOT NULL,
    "SendPaymentLink" boolean NOT NULL,
    "Total" numeric NOT NULL,
    "StatusID" integer DEFAULT 0 NOT NULL,
    "OrderId" text DEFAULT ''::text NOT NULL,
    "PayerId" text DEFAULT ''::text NOT NULL,
    "PaymentId" text DEFAULT ''::text NOT NULL,
    "PaymentSource" text DEFAULT ''::text NOT NULL
);
    DROP TABLE public."Bookings";
       public         heap    postgres    false            �            1259    16515    Bookings_Id_seq    SEQUENCE     �   ALTER TABLE public."Bookings" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Bookings_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    226            �            1259    16508 
   Categories    TABLE     �   CREATE TABLE public."Categories" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    "Image" text NOT NULL
);
     DROP TABLE public."Categories";
       public         heap    postgres    false            �            1259    16507    Categories_Id_seq    SEQUENCE     �   ALTER TABLE public."Categories" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Categories_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    224            �            1259    16524    NewBookings    TABLE     9  CREATE TABLE public."NewBookings" (
    "Id" integer NOT NULL,
    "DateTime" timestamp with time zone NOT NULL,
    "Name" text NOT NULL,
    "Email" text NOT NULL,
    "Phone" text NOT NULL,
    "Names" text NOT NULL,
    "Price" numeric NOT NULL,
    "SubTotal" numeric NOT NULL,
    "Status" text NOT NULL
);
 !   DROP TABLE public."NewBookings";
       public         heap    postgres    false            �            1259    16523    NewBookings_Id_seq    SEQUENCE     �   ALTER TABLE public."NewBookings" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."NewBookings_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    228            �            1259    16532    Services    TABLE       CREATE TABLE public."Services" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "Category" text NOT NULL,
    "Description" text NOT NULL,
    "Image" text NOT NULL,
    "Price" integer DEFAULT 0 NOT NULL,
    "Type" text DEFAULT ''::text NOT NULL
);
    DROP TABLE public."Services";
       public         heap    postgres    false            �            1259    16531    Services_Id_seq    SEQUENCE     �   ALTER TABLE public."Services" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Services_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    230            �            1259    16540    Settings    TABLE     J  CREATE TABLE public."Settings" (
    "Id" integer NOT NULL,
    "FooterContent" text NOT NULL,
    "Address" text NOT NULL,
    "PhoneNumber" text NOT NULL,
    "Email" text NOT NULL,
    "Map" text NOT NULL,
    "Facebook" text NOT NULL,
    "Instagram" text NOT NULL,
    "Twitter" text NOT NULL,
    "Youtube" text NOT NULL
);
    DROP TABLE public."Settings";
       public         heap    postgres    false            �            1259    16539    Settings_Id_seq    SEQUENCE     �   ALTER TABLE public."Settings" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Settings_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    232            �            1259    16548    StaticPages    TABLE       CREATE TABLE public."StaticPages" (
    "Id" integer NOT NULL,
    "Banner" text NOT NULL,
    "Title" text NOT NULL,
    "Content" text DEFAULT ''::text NOT NULL,
    "MetaDescription" text DEFAULT ''::text NOT NULL,
    "MetaTile" text DEFAULT ''::text NOT NULL
);
 !   DROP TABLE public."StaticPages";
       public         heap    postgres    false            �            1259    16547    StaticPages_Id_seq    SEQUENCE     �   ALTER TABLE public."StaticPages" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."StaticPages_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    234            �            1259    16651    Statuses    TABLE     X   CREATE TABLE public."Statuses" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL
);
    DROP TABLE public."Statuses";
       public         heap    postgres    false            �            1259    16650    Statuses_Id_seq    SEQUENCE     �   ALTER TABLE public."Statuses" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Statuses_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    242            �            1259    16556    Testimonals    TABLE     �   CREATE TABLE public."Testimonals" (
    "Id" integer NOT NULL,
    "Image" text NOT NULL,
    "Title" text NOT NULL,
    "Rating" integer NOT NULL,
    "Content" text NOT NULL,
    "Username" text NOT NULL,
    "Designation" text NOT NULL
);
 !   DROP TABLE public."Testimonals";
       public         heap    postgres    false            �            1259    16555    Testimonals_Id_seq    SEQUENCE     �   ALTER TABLE public."Testimonals" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Testimonals_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    236            �            1259    16625    Users    TABLE     �   CREATE TABLE public."Users" (
    "Id" integer NOT NULL,
    "Username" text NOT NULL,
    "PasswordHash" text NOT NULL,
    "Role" text NOT NULL
);
    DROP TABLE public."Users";
       public         heap    postgres    false            �            1259    16624    Users_Id_seq    SEQUENCE     �   ALTER TABLE public."Users" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Users_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    240            �            1259    16402    __EFMigrationsHistory    TABLE     �   CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);
 +   DROP TABLE public."__EFMigrationsHistory";
       public         heap    postgres    false            �            1259    16600    booking_service    TABLE     �   CREATE TABLE public.booking_service (
    id integer NOT NULL,
    booking_id integer NOT NULL,
    service_id integer NOT NULL,
    service_name text NOT NULL,
    price numeric NOT NULL,
    quantity integer DEFAULT 1 NOT NULL
);
 #   DROP TABLE public.booking_service;
       public         heap    postgres    false            �            1259    16599    booking_service_id_seq    SEQUENCE     �   CREATE SEQUENCE public.booking_service_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 -   DROP SEQUENCE public.booking_service_id_seq;
       public          postgres    false    238            z           0    0    booking_service_id_seq    SEQUENCE OWNED BY     Q   ALTER SEQUENCE public.booking_service_id_seq OWNED BY public.booking_service.id;
          public          postgres    false    237            �            1259    16436 
   department    TABLE     p   CREATE TABLE public.department (
    departmentid integer NOT NULL,
    departmentname character varying(90)
);
    DROP TABLE public.department;
       public         heap    postgres    false            �            1259    16435    department_departmentid_seq    SEQUENCE     �   CREATE SEQUENCE public.department_departmentid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 2   DROP SEQUENCE public.department_departmentid_seq;
       public          postgres    false    218            {           0    0    department_departmentid_seq    SEQUENCE OWNED BY     [   ALTER SEQUENCE public.department_departmentid_seq OWNED BY public.department.departmentid;
          public          postgres    false    217            �            1259    16441    employee    TABLE     �   CREATE TABLE public.employee (
    employeeid integer NOT NULL,
    employeename character varying(90),
    department character varying(90),
    dateofjoining date
);
    DROP TABLE public.employee;
       public         heap    postgres    false            �            1259    16440    employee_employeeid_seq    SEQUENCE     �   CREATE SEQUENCE public.employee_employeeid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 .   DROP SEQUENCE public.employee_employeeid_seq;
       public          postgres    false    220            |           0    0    employee_employeeid_seq    SEQUENCE OWNED BY     S   ALTER SEQUENCE public.employee_employeeid_seq OWNED BY public.employee.employeeid;
          public          postgres    false    219            �            1259    16399    persons    TABLE     V   CREATE TABLE public.persons (
    personid integer,
    name character varying(25)
);
    DROP TABLE public.persons;
       public         heap    postgres    false            �            1259    16446    test    TABLE     W   CREATE TABLE public.test (
    id integer NOT NULL,
    name character varying(255)
);
    DROP TABLE public.test;
       public         heap    postgres    false            �            1259    16445    test_id_seq    SEQUENCE     �   CREATE SEQUENCE public.test_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 "   DROP SEQUENCE public.test_id_seq;
       public          postgres    false    222            }           0    0    test_id_seq    SEQUENCE OWNED BY     ;   ALTER SEQUENCE public.test_id_seq OWNED BY public.test.id;
          public          postgres    false    221            �           2604    16603    booking_service id    DEFAULT     x   ALTER TABLE ONLY public.booking_service ALTER COLUMN id SET DEFAULT nextval('public.booking_service_id_seq'::regclass);
 A   ALTER TABLE public.booking_service ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    238    237    238            �           2604    16439    department departmentid    DEFAULT     �   ALTER TABLE ONLY public.department ALTER COLUMN departmentid SET DEFAULT nextval('public.department_departmentid_seq'::regclass);
 F   ALTER TABLE public.department ALTER COLUMN departmentid DROP DEFAULT;
       public          postgres    false    217    218    218            �           2604    16444    employee employeeid    DEFAULT     z   ALTER TABLE ONLY public.employee ALTER COLUMN employeeid SET DEFAULT nextval('public.employee_employeeid_seq'::regclass);
 B   ALTER TABLE public.employee ALTER COLUMN employeeid DROP DEFAULT;
       public          postgres    false    219    220    220            �           2604    16449    test id    DEFAULT     b   ALTER TABLE ONLY public.test ALTER COLUMN id SET DEFAULT nextval('public.test_id_seq'::regclass);
 6   ALTER TABLE public.test ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    222    221    222            c          0    16516    Bookings 
   TABLE DATA           �   COPY public."Bookings" ("Id", "User", "AppointmentDate", "AppointmentTime", "Address1", "Address2", "City", "State", "Pincode", "Services", "SendPaymentLink", "Total", "StatusID", "OrderId", "PayerId", "PaymentId", "PaymentSource") FROM stdin;
    public          postgres    false    226   F|       a          0    16508 
   Categories 
   TABLE DATA           L   COPY public."Categories" ("Id", "Name", "Description", "Image") FROM stdin;
    public          postgres    false    224   _�       e          0    16524    NewBookings 
   TABLE DATA           {   COPY public."NewBookings" ("Id", "DateTime", "Name", "Email", "Phone", "Names", "Price", "SubTotal", "Status") FROM stdin;
    public          postgres    false    228   P�       g          0    16532    Services 
   TABLE DATA           g   COPY public."Services" ("Id", "Name", "Category", "Description", "Image", "Price", "Type") FROM stdin;
    public          postgres    false    230   m�       i          0    16540    Settings 
   TABLE DATA           �   COPY public."Settings" ("Id", "FooterContent", "Address", "PhoneNumber", "Email", "Map", "Facebook", "Instagram", "Twitter", "Youtube") FROM stdin;
    public          postgres    false    232   ��       k          0    16548    StaticPages 
   TABLE DATA           j   COPY public."StaticPages" ("Id", "Banner", "Title", "Content", "MetaDescription", "MetaTile") FROM stdin;
    public          postgres    false    234   ��       s          0    16651    Statuses 
   TABLE DATA           2   COPY public."Statuses" ("Id", "Name") FROM stdin;
    public          postgres    false    242   ˅       m          0    16556    Testimonals 
   TABLE DATA           o   COPY public."Testimonals" ("Id", "Image", "Title", "Rating", "Content", "Username", "Designation") FROM stdin;
    public          postgres    false    236   �       q          0    16625    Users 
   TABLE DATA           K   COPY public."Users" ("Id", "Username", "PasswordHash", "Role") FROM stdin;
    public          postgres    false    240   5�       Y          0    16402    __EFMigrationsHistory 
   TABLE DATA           R   COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
    public          postgres    false    216   r�       o          0    16600    booking_service 
   TABLE DATA           d   COPY public.booking_service (id, booking_id, service_id, service_name, price, quantity) FROM stdin;
    public          postgres    false    238   J�       [          0    16436 
   department 
   TABLE DATA           B   COPY public.department (departmentid, departmentname) FROM stdin;
    public          postgres    false    218   �       ]          0    16441    employee 
   TABLE DATA           W   COPY public.employee (employeeid, employeename, department, dateofjoining) FROM stdin;
    public          postgres    false    220   3�       X          0    16399    persons 
   TABLE DATA           1   COPY public.persons (personid, name) FROM stdin;
    public          postgres    false    215   v�       _          0    16446    test 
   TABLE DATA           (   COPY public.test (id, name) FROM stdin;
    public          postgres    false    222   ��       ~           0    0    Bookings_Id_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public."Bookings_Id_seq"', 167, true);
          public          postgres    false    225                       0    0    Categories_Id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public."Categories_Id_seq"', 40, true);
          public          postgres    false    223            �           0    0    NewBookings_Id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public."NewBookings_Id_seq"', 1, false);
          public          postgres    false    227            �           0    0    Services_Id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public."Services_Id_seq"', 10, true);
          public          postgres    false    229            �           0    0    Settings_Id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."Settings_Id_seq"', 3, true);
          public          postgres    false    231            �           0    0    StaticPages_Id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public."StaticPages_Id_seq"', 5, true);
          public          postgres    false    233            �           0    0    Statuses_Id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public."Statuses_Id_seq"', 1, false);
          public          postgres    false    241            �           0    0    Testimonals_Id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public."Testimonals_Id_seq"', 21, true);
          public          postgres    false    235            �           0    0    Users_Id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public."Users_Id_seq"', 3, true);
          public          postgres    false    239            �           0    0    booking_service_id_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public.booking_service_id_seq', 148, true);
          public          postgres    false    237            �           0    0    department_departmentid_seq    SEQUENCE SET     I   SELECT pg_catalog.setval('public.department_departmentid_seq', 4, true);
          public          postgres    false    217            �           0    0    employee_employeeid_seq    SEQUENCE SET     E   SELECT pg_catalog.setval('public.employee_employeeid_seq', 3, true);
          public          postgres    false    219            �           0    0    test_id_seq    SEQUENCE SET     :   SELECT pg_catalog.setval('public.test_id_seq', 18, true);
          public          postgres    false    221            �           2606    16522    Bookings PK_Bookings 
   CONSTRAINT     X   ALTER TABLE ONLY public."Bookings"
    ADD CONSTRAINT "PK_Bookings" PRIMARY KEY ("Id");
 B   ALTER TABLE ONLY public."Bookings" DROP CONSTRAINT "PK_Bookings";
       public            postgres    false    226            �           2606    16514    Categories PK_Categories 
   CONSTRAINT     \   ALTER TABLE ONLY public."Categories"
    ADD CONSTRAINT "PK_Categories" PRIMARY KEY ("Id");
 F   ALTER TABLE ONLY public."Categories" DROP CONSTRAINT "PK_Categories";
       public            postgres    false    224            �           2606    16530    NewBookings PK_NewBookings 
   CONSTRAINT     ^   ALTER TABLE ONLY public."NewBookings"
    ADD CONSTRAINT "PK_NewBookings" PRIMARY KEY ("Id");
 H   ALTER TABLE ONLY public."NewBookings" DROP CONSTRAINT "PK_NewBookings";
       public            postgres    false    228            �           2606    16538    Services PK_Services 
   CONSTRAINT     X   ALTER TABLE ONLY public."Services"
    ADD CONSTRAINT "PK_Services" PRIMARY KEY ("Id");
 B   ALTER TABLE ONLY public."Services" DROP CONSTRAINT "PK_Services";
       public            postgres    false    230            �           2606    16546    Settings PK_Settings 
   CONSTRAINT     X   ALTER TABLE ONLY public."Settings"
    ADD CONSTRAINT "PK_Settings" PRIMARY KEY ("Id");
 B   ALTER TABLE ONLY public."Settings" DROP CONSTRAINT "PK_Settings";
       public            postgres    false    232            �           2606    16554    StaticPages PK_StaticPages 
   CONSTRAINT     ^   ALTER TABLE ONLY public."StaticPages"
    ADD CONSTRAINT "PK_StaticPages" PRIMARY KEY ("Id");
 H   ALTER TABLE ONLY public."StaticPages" DROP CONSTRAINT "PK_StaticPages";
       public            postgres    false    234            �           2606    16657    Statuses PK_Statuses 
   CONSTRAINT     X   ALTER TABLE ONLY public."Statuses"
    ADD CONSTRAINT "PK_Statuses" PRIMARY KEY ("Id");
 B   ALTER TABLE ONLY public."Statuses" DROP CONSTRAINT "PK_Statuses";
       public            postgres    false    242            �           2606    16562    Testimonals PK_Testimonals 
   CONSTRAINT     ^   ALTER TABLE ONLY public."Testimonals"
    ADD CONSTRAINT "PK_Testimonals" PRIMARY KEY ("Id");
 H   ALTER TABLE ONLY public."Testimonals" DROP CONSTRAINT "PK_Testimonals";
       public            postgres    false    236            �           2606    16631    Users PK_Users 
   CONSTRAINT     R   ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");
 <   ALTER TABLE ONLY public."Users" DROP CONSTRAINT "PK_Users";
       public            postgres    false    240            �           2606    16406 .   __EFMigrationsHistory PK___EFMigrationsHistory 
   CONSTRAINT     {   ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");
 \   ALTER TABLE ONLY public."__EFMigrationsHistory" DROP CONSTRAINT "PK___EFMigrationsHistory";
       public            postgres    false    216            �           2606    16607 $   booking_service booking_service_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public.booking_service
    ADD CONSTRAINT booking_service_pkey PRIMARY KEY (id);
 N   ALTER TABLE ONLY public.booking_service DROP CONSTRAINT booking_service_pkey;
       public            postgres    false    238            �           2606    16451    test test_pkey 
   CONSTRAINT     L   ALTER TABLE ONLY public.test
    ADD CONSTRAINT test_pkey PRIMARY KEY (id);
 8   ALTER TABLE ONLY public.test DROP CONSTRAINT test_pkey;
       public            postgres    false    222            �           2620    16614 %   Bookings booking_service_sync_trigger    TRIGGER     �   CREATE TRIGGER booking_service_sync_trigger AFTER INSERT OR DELETE OR UPDATE ON public."Bookings" FOR EACH ROW EXECUTE FUNCTION public.sync_booking_service();
 @   DROP TRIGGER booking_service_sync_trigger ON public."Bookings";
       public          postgres    false    262    226            �           2606    16618 /   booking_service booking_service_booking_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.booking_service
    ADD CONSTRAINT booking_service_booking_id_fkey FOREIGN KEY (booking_id) REFERENCES public."Bookings"("Id") ON DELETE CASCADE;
 Y   ALTER TABLE ONLY public.booking_service DROP CONSTRAINT booking_service_booking_id_fkey;
       public          postgres    false    238    4790    226            c   	  x��mo�6�_+�B��>S�;w[���4i�nZ�c-z�,�3�~�Qq�Ĥd)�6� v$˒���xw2D��D*#D<x � p��w �́H�t��9�Lf����J�q���v�: @����(�F��|���]�^���;Y���<��BQɻ�\�=�i]f�|&#�U�݉$����yX�E�zS��Y�͡q*�O|x�V�}�0��#P�^ ��c<�� ��Ϗ3Q��E���箿,vE�BpA���N��Z�)��H���(�Pm�����EVŕ�t���r��8��$����$[���+u9"���zq\��JeVWǶRT�&�M�xO�^���!I8��?=E��YF���b�zܪ�G��#~ ���Dq�ϓ{Q	狜W�|��/y󀧅�qj6@�K��"��$��m�D���	�&MjJ�\�N8�5��y6A��X��\Z�M|�2�fᬕ^4<�S��2� �=!��&����"<����^#8�m$ܴ�< l6?a���x�jyCq�Q��ٽ���(���k���Ax%t�@��XK�"���s���^��թ9Y��ܲ|K��=0b��LY �T��I{q�F�����������!��n�>E�m)Ģ�֯j��8��~�?�ܛN��%eO�Ri:@a(�O��8Х�ˁ �~	|�o("���������?ݾ�в=F�y�A8v�r�'~�[ZP����G�3�(����+�!Hz�tPz�?X��Q�S���y5r[�GLPZ/ҩ���d(m�|����<��o�87���Q�bKs_����A�r�n+u�dp��z�~�F2����aq��s
��1�[�:Wu��wut�������ŏ-�/�bǒ<IbI���[_��fn*��-s9�4.U�*R7̛���\K�.Զ��F�� ��S�R���{���q�Ff@`�x����90�A/̽V-�<�5]B8��bzpR��X���g��K�����ň��pDF=�^b�ۤ�]rum���������^��6kh��O�Y�Ϭ� ӚȱL;�6���U�"������u��(l�&2�I[���M74�%c��`�A�M��j+�0���&Ў�5���Z~E�7'�|���5�����k�\�[C����-�r�o0>��sPk;��f��Y�� ��|��3v����Akʜ�����u~�>Mg��o�y�cԉ�R�i�w��q���ol����{����h�{lsOGs�狳��� �|��      a   �   x���;N�@��z}
�P�5��ϒ��XB����	y���8"�� �>��Ff9j5�~��(;]�3�R P	�<W����57V���f��/�K�z�z��f@���>��Ew's:�?崘y�+�Dm�M�P@���o�M��!��O��+�)��Sk�Cq������D�8O�[,��Zu�Q���h�|����4�К�m)B�1@�d9����cҏ2]�}��s_      e      x������ � �      g     x���MK�@����S�QS&����Z�KA6��M4o$)�7E�Y=?�c�a�x����cO}�L}3ה��ҟC;��v�G�.k�p��t2֕�%B�$A���G]��>��'k^����q��+.P(@(yXx�;mv�w�#�Y)$;6��Ra�����e��e?�i�_�yì��4Y�փ�Uoȁ eQ���`"�hܟ5�Dr���+�@���h}�1��Q8�_�g�:t��ݵ�LWԑ�����OZ/���%��R_�u@MY�������6��a�2�vEQ|�ܧ�      i   �   x�͐�N�0D��W��IՔ�R(B��\6�ƶ�������"!$��hq���G��d�1���K)ű.���<�p=����S	�-<�)82jxdKx�{�K�1Z��V��ŮQ�e�����l��mO�ar�Ӿ��e��Y;����*�=���)?�	�_��y�kf6�/І�QG�g�u�X	Ql���Á���nA�T׶�씼X�)��R�"��� u��      k   #  x����j1�k�S�
I�C�֕&EH\����N�m�c]~��ap�-v�a�Q(���~��W�p(� 1FB�;�i
��q�r��wo��
)׌jt�Cvb���������z"��q$z��in���_��m{>���s�%���3�+�S՗s�������t���RI�C9��F7����\����Qi�'fb��M�x����-��M�sC)�FC���h&�^��aX�]�`5S!��.�����T!�y�(%T
#����]���t�      s   3   x�3�t��K�,�MM�2���+(�ON-.�2�tK��
�p�&��b���� t�P      m     x��лj�0�Y~
��p�d�GC�P�B�%�ִN�v��׎�s@G���_\��w1o6ZW�:�!��Ck,���[�*n��x�u�����V���0A�F9+�.�.�]ܑ˅��@�W��a�Fd �s �C��#x��B�c����|F�uӬW�3�,?�t�X����9��/~E�=���?�����IZɪ5�W=#��0��J��\>�G�'�<v��/u�9�5�<\�q�+>�<o����	 �ke���g�A*�>�o{"�ϼ�bSE� �}�      q   -   x�3�t�H�MM�R�F�&��)��y\Ɯ��9��Y"F��� �%�      Y   �   x�e��� ���.3��ӻ,i��Ibܢ���]���>�R��%he�pR����P�
���)\nC��w�HiN]��8)@p{����ϵ����,"��Ƨp̛?�)�/i�x��2�@��ki��
+ �x�RA���Q����d.�iW0�_#I��/�m�I��j��2��R(�yh9H�wv=�UU} �kj�      o   �  x�u��NB1E��W��w�-�K��nI4BH���;�; &�;�����*��t�~�^v������n[Z��t>��>��s��Z��-"��y]���*����n�ڋ�]Ȍ�,��k�}`��9W�SA.7�����$���9���0�0���E!ܱ\/ħR��R��뇑ƛ����
b�~l� ْ7��\G�FA.�!�13�4�I��15�6T#8�T�+x��]��F",�$�����ݡK�]w���������i(�=���ow�9�CW�x��0ڿ��ah75��Ё��9�&(��&(݌����av�c$�x� �W����P�<��Ȁr�Ab�(���N&{�4}��#�'�r���r�H)gN��1j�������^j��E��'      [   0   x�3���2��K-WpI-H,*�M�+�2�,.-(�/*��K����� ���      ]   3   x�3�L�H�MM�,.-(�/*��K�4����54�50�2�Ig� ��qqq `I      X      x������ � �      _   U   x�3�t��LN�2�t�O�2�t�H,��L��LK�24��K-WI-.Q�,I��24�D�sf'�ddfsY9�@!��b�=... `XO     