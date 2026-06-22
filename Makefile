# ============================================================================
#  Turbohesap — geliştirme komutları
#  Temel komut: `make run`  (frontend derlenir, API + Web başlar, URL'ler yazılır)
#  Durdurma:    `make stop`
# ============================================================================

API_PROJ   := src/Turbohesap.Api/Turbohesap.Api.csproj
WEB_PROJ   := src/Turbohesap.Web/Turbohesap.Web.csproj
FRONTEND   := src/Turbohesap.Web/Frontend
API_PORT   := 5020
WEB_PORT   := 5180
RUN_DIR    := .run
ENV        := Development

GREEN := \033[0;32m
BLUE  := \033[0;34m
DIM   := \033[2m
NC    := \033[0m

.DEFAULT_GOAL := help
.PHONY: help install front front-watch build run start stop restart status logs api web db migrate clean

help: ## Komutları listele
	@echo "$(BLUE)Turbohesap$(NC) — kullanılabilir komutlar:"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | \
		awk 'BEGIN {FS = ":.*?## "}; {printf "  $(GREEN)%-12s$(NC) %s\n", $$1, $$2}'

install: ## Tüm bağımlılıkları kur (dotnet restore + npm install)
	dotnet restore
	cd $(FRONTEND) && npm install

front: ## Frontend'i derle (wwwroot/turbohesap.{css,js})
	cd $(FRONTEND) && npm run build

front-watch: ## Frontend'i izleyerek derle (geliştirme)
	cd $(FRONTEND) && npm run watch

build: front ## Frontend + tüm .NET projelerini derle
	dotnet build

db: migrate ## 'migrate' için takma ad
migrate: ## Bekleyen EF Core migration'larını uygula
	dotnet ef database update --project $(API_PROJ)

run: front ## Frontend'i derle, API + Web'i arka planda başlat, URL'leri yaz
	@mkdir -p $(RUN_DIR)
	@echo "$(DIM)API başlatılıyor (port $(API_PORT))…$(NC)"
	@ASPNETCORE_URLS=http://localhost:$(API_PORT) ASPNETCORE_ENVIRONMENT=$(ENV) \
		dotnet run --project $(API_PROJ) --no-launch-profile > $(RUN_DIR)/api.log 2>&1 & echo $$! > $(RUN_DIR)/api.pid
	@echo "$(DIM)Web başlatılıyor (port $(WEB_PORT))…$(NC)"
	@ASPNETCORE_URLS=http://localhost:$(WEB_PORT) ASPNETCORE_ENVIRONMENT=$(ENV) \
		dotnet run --project $(WEB_PROJ) --no-launch-profile > $(RUN_DIR)/web.log 2>&1 & echo $$! > $(RUN_DIR)/web.pid
	@echo ""
	@echo "$(GREEN)Turbohesap çalışıyor:$(NC)"
	@echo "  Web        : $(BLUE)http://localhost:$(WEB_PORT)$(NC)"
	@echo "  API        : $(BLUE)http://localhost:$(API_PORT)$(NC)"
	@echo "  API Docs   : $(BLUE)http://localhost:$(API_PORT)/scalar/v1$(NC)"
	@echo "  OpenAPI    : $(BLUE)http://localhost:$(API_PORT)/openapi/v1.json$(NC)"
	@echo ""
	@echo "$(DIM)Loglar: make logs   ·   Durdur: make stop$(NC)"

start: run ## 'run' için takma ad

stop: ## Çalışan API + Web süreçlerini durdur
	@-[ -f $(RUN_DIR)/api.pid ] && kill `cat $(RUN_DIR)/api.pid` 2>/dev/null || true
	@-[ -f $(RUN_DIR)/web.pid ] && kill `cat $(RUN_DIR)/web.pid` 2>/dev/null || true
	@-lsof -ti tcp:$(API_PORT) 2>/dev/null | xargs kill -9 2>/dev/null || true
	@-lsof -ti tcp:$(WEB_PORT) 2>/dev/null | xargs kill -9 2>/dev/null || true
	@rm -f $(RUN_DIR)/*.pid
	@echo "$(GREEN)Durduruldu.$(NC)"

restart: stop run ## Durdur ve yeniden başlat

status: ## Çalışma durumunu göster
	@lsof -ti tcp:$(API_PORT) >/dev/null 2>&1 && echo "API : $(GREEN)çalışıyor$(NC) (:$(API_PORT))" || echo "API : durmuş"
	@lsof -ti tcp:$(WEB_PORT) >/dev/null 2>&1 && echo "Web : $(GREEN)çalışıyor$(NC) (:$(WEB_PORT))" || echo "Web : durmuş"

logs: ## Çalışan süreçlerin loglarını izle
	@tail -f $(RUN_DIR)/api.log $(RUN_DIR)/web.log

api: front ## Sadece API'yi (ön planda) çalıştır
	ASPNETCORE_URLS=http://localhost:$(API_PORT) ASPNETCORE_ENVIRONMENT=$(ENV) dotnet run --project $(API_PROJ) --no-launch-profile

web: front ## Sadece Web'i (ön planda) çalıştır
	ASPNETCORE_URLS=http://localhost:$(WEB_PORT) ASPNETCORE_ENVIRONMENT=$(ENV) dotnet run --project $(WEB_PROJ) --no-launch-profile

clean: stop ## Süreçleri durdur ve derleme çıktılarını temizle
	dotnet clean
	rm -rf $(RUN_DIR)
	find src -type d -name bin -prune -exec rm -rf {} + 2>/dev/null || true
	find src -type d -name obj -prune -exec rm -rf {} + 2>/dev/null || true
	@echo "$(GREEN)Temizlendi.$(NC)"
