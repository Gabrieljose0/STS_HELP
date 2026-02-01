-- ============================================================
-- SCRIPT DE CRIAÇÃO DO BANCO DE DADOS - STS HELP
-- ============================================================

-- 1. TABELAS AUXILIARES (Domínios)
-- ============================================================

CREATE TABLE categoria (
  id_categoria SERIAL PRIMARY KEY,
  nm_categoria VARCHAR(50) NOT NULL
);

CREATE TABLE prioridades (
  id_prioridade SERIAL PRIMARY KEY, -- Corrigido de 'id_pripridade'
  nm_prioridade VARCHAR(50) NOT NULL
);

CREATE TABLE status (
  id_status SERIAL PRIMARY KEY,
  nm_status VARCHAR(50) NOT NULL
);

-- 2. TABELA DE USUÁRIOS (Integrada com Supabase Auth)
-- ============================================================

CREATE TABLE usuarios (
  id SERIAL PRIMARY KEY,
  nome VARCHAR(100) NOT NULL,
  email VARCHAR(100) NOT NULL,
  tipo_usuario VARCHAR(50) DEFAULT 'Pendente', -- Antigo 'departamento'
  situacao_usuario BOOLEAN DEFAULT TRUE,
  auth_id UUID REFERENCES auth.users(id) ON DELETE CASCADE -- Vínculo com Supabase
);

-- 3. TABELA DE CHAMADOS (Tabela Principal)
-- ============================================================

CREATE TABLE chamados (
    id_chamados SERIAL PRIMARY KEY,
    titulo VARCHAR (50),
    texto VARCHAR (500),
    dt_abertura TIMESTAMP DEFAULT NOW(),
    dt_fechamento TIMESTAMP,
    
    -- Chaves Estrangeiras
    categoria INT REFERENCES categoria (id_categoria),
    prioridade INT REFERENCES prioridades (id_prioridade),
    status INT REFERENCES status (id_status),
    
    -- Relacionamento com Usuários
    usuario INT REFERENCES usuarios (id), -- Quem abriu
    id_tecnico INT REFERENCES usuarios (id) -- Técnico responsável
);

-- ============================================================
-- CARGA DE DADOS INICIAIS (População)
-- ============================================================

INSERT INTO categoria (nm_categoria) VALUES 
('Problemas de Rede'),
('Erro no Sistema'),
('Acesso Negado'),
('Solicitação de Equipamento'),
('Solicitação de Software');

INSERT INTO prioridades (nm_prioridade) VALUES 
('Baixa'),
('Media'),
('Alta');

INSERT INTO status (nm_status) VALUES 
('Aberto'),
('Em Atendimento'),
('Concluido');

-- Usuários de Teste (Sem vínculo com Auth real para facilitar testes locais)
INSERT INTO usuarios (nome, email, tipo_usuario, situacao_usuario) VALUES
('Gestor Padrão', 'gestor@teste.com', 'Gestor', true),
('Técnico Padrão', 'tecnico@teste.com', 'Tecnico', true),
('Colaborador Padrão', 'user@teste.com', 'Colaborador', true);

-- Chamados de Exemplo
INSERT INTO chamados (titulo, texto, categoria, prioridade, usuario, status, dt_abertura) VALUES
('Sem acesso ao sistema ERP', 'Não estou conseguindo acessar o sistema ERP desde hoje pela manhã.', 3, 3, 3, 1, '2025-05-17 09:12:00'),
('Solicitação de notebook', 'Solicito um notebook para trabalho remoto.', 4, 2, 3, 2, '2025-05-16 14:32:00'),
('Erro ao salvar planilha', 'O sistema apresenta erro ao tentar salvar planilhas na rede.', 2, 3, 3, 3, '2025-05-15 11:00:00');


-- ============================================================
-- AUTOMAÇÃO SUPABASE (Triggers e Functions)
-- ============================================================
-- Esta parte garante que quando um usuário é criado no Authentication do Supabase,
-- ele seja copiado automaticamente para a tabela pública 'usuarios'.

CREATE OR REPLACE FUNCTION public.handle_new_user()
RETURNS trigger
LANGUAGE plpgsql
SECURITY DEFINER
AS $$
BEGIN
  INSERT INTO public.usuarios (
    auth_id, 
    email, 
    nome, 
    tipo_usuario,
    situacao_usuario
  )
  VALUES (
    NEW.id,
    NEW.email,
    -- Pega do metadata ou define padrão se vier vazio
    COALESCE(NEW.raw_user_meta_data->>'nome', 'Nome Pendente'), 
    COALESCE(NEW.raw_user_meta_data->>'tipo_usuario', 'Pendente'),
    TRUE
  );
  RETURN NEW;
END;
$$;

-- Recria o Trigger
DROP TRIGGER IF EXISTS on_auth_user_created ON auth.users;
CREATE TRIGGER on_auth_user_created
  AFTER INSERT ON auth.users
  FOR EACH ROW EXECUTE PROCEDURE public.handle_new_user();
